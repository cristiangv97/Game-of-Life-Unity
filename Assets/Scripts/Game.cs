using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using TMPro;

public class Game : MonoBehaviour
{
    private static int SCREEN_WIDTH = 500;
    private static int SCREEN_HEIGHT = 500;

    public HUD hud;
    private int numIte = 0;

    public float speed = 1f;

    private float timer = 0f;

    public bool simulationEnabled = false;

    public bool normalGameM = true;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("SavePattern", SavePattern);
        EventManager.StartListening("LoadPattern", LoadPattern);
        EventManager.StartListening("CambiarColor", CambiarColor);
        EventManager.StartListening("SalirDialog", SalirDialog);
        EventManager.StartListening("CambiarTamano", CambiarTamano);

        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationEnabled)
        {
            if (timer >= speed)
            {
                timer = 0f;
                CountNeighbors();

                PopulationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        UserInput();
    }

    private void SavePattern()
    {
        string path = "patterns";

        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

        Pattern pattern = new Pattern();
        pattern.patternString = new string[SCREEN_HEIGHT];

        string patternString = null;

        for(int y = 0; y < SCREEN_HEIGHT; y++)
        {
            patternString = null;
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                if (!grid[x, y].isAlive)
                {
                    patternString += "0";
                }
                else
                {
                    patternString += "1";
                }
            }
            pattern.patternString[y] = patternString;
        }
        pattern.numeroFilas = SCREEN_HEIGHT;
        pattern.numeroColumnas = SCREEN_WIDTH;

        XmlSerializer serializer = new XmlSerializer(typeof(Pattern));

        StreamWriter writer = new StreamWriter(path + "/" + hud.saveDialog.patternName.text + ".xml");
        serializer.Serialize(writer.BaseStream, pattern);
        writer.Close();

        // Debug.Log(pattern.patternString);
    }

    private void LoadPattern()
    {
        RemoveCells();

        string path = "patterns";
        if (!Directory.Exists(path))
        {
            return;
        }


        XmlSerializer serializer = new XmlSerializer(typeof(Pattern));
        string patternName = hud.loadDialog.patternName.options[hud.loadDialog.patternName.value].text;
        path = path + "/" + patternName + ".xml";

        StreamReader reader = new StreamReader(path);
        Pattern pattern = (Pattern)serializer.Deserialize(reader.BaseStream);
        reader.Close();

        int numCel = 0;

        SCREEN_HEIGHT = pattern.numeroFilas;
        SCREEN_WIDTH = pattern.numeroColumnas;

        grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell_4", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                if (pattern.patternString[y][x].Equals('1'))
                {
                    grid[x, y].SetAlive(true);
                    numCel++;
                }
                else
                {
                    grid[x, y].SetAlive(false);
                }
            }
        }

        hud.actualizarCelulas(numCel);
        // phCel.text = numCel.ToString();

    }

    private void CambiarColor()
    {
        var clones = GameObject.FindGameObjectsWithTag("Celula");
        foreach (var clone in clones)
        {
            float rojo = hud.colorDialog.sliderRojo.value;
            float verde = hud.colorDialog.sliderVerde.value;
            float azul = hud.colorDialog.sliderAzul.value; ;
            clone.GetComponent<SpriteRenderer>().color = new Color(rojo, verde, azul, 1);
        }
    }

    void UserInput()
    {
        if (!hud.isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(mousePoint.x);
                int y = Mathf.RoundToInt(mousePoint.y);

                if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT)
                {
                    grid[x, y].SetAlive(!grid[x, y].isAlive);
                }
            }
            else if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Space))
            {
                simulationEnabled = !simulationEnabled;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                hud.showSaveDialog();
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                hud.showLoadDialog();
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                RenewCells();
            }
            else if (Input.GetKeyUp(KeyCode.Period))
            {
                CountNeighbors();
                PopulationControl();
            }
            else if (Input.GetKeyUp(KeyCode.LeftBracket))
            {
                if (speed < 4f)
                {
                    speed += 0.01f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.RightBracket))
            {
                if (speed >= 0f)
                {
                    speed -= 0.01f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.N))
            {
                simulationEnabled = false;
                normalGameM = !normalGameM;
            }
            else if (Input.GetKeyUp(KeyCode.T))
            {
                simulationEnabled = false;
                hud.showSizeDialog();
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                simulationEnabled = false;
                hud.showColorDialog();
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                simulationEnabled = false;
                hud.showSalirDialog();
            }
        }

    }

    // private void FixedUpdate()
    // {
    //     CountNeighbors();

    //     PopulationControl();
    // }
    private void CambiarTamano()
    {
        RemoveCells();
        SCREEN_WIDTH = (int) hud.sizeDialog.sliderColumnas.value;
        // SCREEN_WIDTH = 150;
        // Debug.Log("Columnas: " + SCREEN_WIDTH);
        SCREEN_HEIGHT = (int) hud.sizeDialog.sliderFilas.value;
        // SCREEN_HEIGHT = 110;

        grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];
        // Debug.Log("Filas: " + SCREEN_HEIGHT);

        PlaceCells();

    }

    void PlaceCells()
    {
        int numCel = 0;
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell_4", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                if (RandomAliveCell())
                {
                    grid[x, y].SetAlive(true);
                    numCel++;
                }
                else
                {
                    grid[x, y].SetAlive(false);
                }
            }
        }
        hud.actualizarTodo(numCel, 0);
        // phCel.text = numCel.ToString();
    }

    bool RandomAliveCell()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand > 75)
        {
            return true;
        }
        return false;
    }

    void CountNeighbors()
    {
        for(int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for(int x = 0; x < SCREEN_WIDTH ; x++)
            {
                int numNeighbors = 0;

                /* ESFERICO */
                if (!normalGameM)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (MyMod(i + x - 1, SCREEN_WIDTH) == x && MyMod(j + y - 1, SCREEN_HEIGHT) == y)
                            {
                                continue;
                            }
                            if (grid[MyMod(i + x - 1, SCREEN_WIDTH), MyMod(j + y - 1, SCREEN_HEIGHT)].isAlive)
                            {
                                numNeighbors++;
                            }
                        }
                    }
                }
                else
                {
                    /* NORMAL */
                    // NORTH
                    if(y + 1 < SCREEN_HEIGHT)
                    {
                        if (grid[x, y + 1].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // EAST
                    if (x + 1 < SCREEN_WIDTH)
                    {
                        if (grid[x+1, y].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // SOUTH
                    if (y - 1 >= 0)
                    {
                        if (grid[x, y - 1].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // WEST 
                    if (x - 1 >= 0)
                    {
                        if (grid[x - 1, y].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // NORTH-EAST
                    if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT)
                    {
                        if (grid[x + 1, y + 1].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // NORTH-WEST
                    if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT)
                    {
                        if (grid[x - 1, y + 1].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // SOUTH-EAST
                    if (x + 1 < SCREEN_WIDTH && y - 1 >= 0)
                    {
                        if (grid[x + 1, y - 1].isAlive)
                        {
                            numNeighbors++;
                        }
                    }

                    // SOUTH-WEST
                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        if (grid[x - 1, y - 1].isAlive)
                        {
                            numNeighbors++;
                        }
                    }
                }

                grid[x, y].numNeighbors = numNeighbors;
            }
        }
    }

    void SalirDialog()
    {
        Application.Quit();
    }

    void RenewCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                grid[x, y].SetAlive(RandomAliveCell());
            }
        }
    }

    void PopulationControl()
    {
        int numCel = 0;
        for(int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                if(grid[x, y].isAlive)
                {
                    if (grid[x, y].numNeighbors != 2 && grid[x, y].numNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false);
                    }
                    else
                    {
                        numCel++;
                    }
                }
                else
                {
                    if (grid[x, y].numNeighbors == 3)
                    {
                        numCel++;
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
        numIte++;

        hud.actualizarTodo(numCel, numIte);
        // phCel.text = numCel.ToString();
        // phIte.text = numIte.ToString();
    }

    void RemoveCells()
    {
        var celulas = GameObject.FindGameObjectsWithTag("Celula");

        foreach(var celula in celulas)
        {
            Destroy(celula);
        }
    }

    int MyMod(int num, int mod)
    {
        if (num >= 0)
        {
            return num % mod;
        }
        return mod + num;
    }
}
