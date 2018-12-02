using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulation : MonoBehaviour
{
    public GameObject Beehive;
    public GameObject BeeKillerObject;
    public GameObject GameOverMenuObject;
    public GameObject PlayButton;
    public GameObject SoundButton;
    public GameObject ExitButton;
    public GameObject Headline;
    public Text ScoreText;
    public Text SacrificesText;
    public Text FinalScore;
    public Text HighscoreText;
    public Image ScoreImage;
    public Image SacrificesImage;
    public Image FinalScoreImage;
    public Image HighscoreImage;
    public int BeeCount = 30;
    public float KillRadiusPercentage = 0.05f;

    private const string FlowerPath = "Prefabs/Flower";
    private const string BeePath = "Prefabs/Bee";
    private const string BackgroundPath = "Prefabs/Background";
    private const string SpiderPath = "Prefabs/Spider";
    private const string DeadBeePath = "Prefabs/DeadBee";
    private const string DeadSpiderPath = "Prefabs/DeadSpider";
    private List<GameObject> flowers;
    public List<GameObject> bees;
    public List<GameObject> spiders;

    private float timer;
    private float generationTime = 2;
    private float spidersPerSpawn = 1;
    private float worldWidth;
    private float worldHeight;
    private float killRadius;

    private BeeKiller beeKiller;

    private int killedSpiders;
    private int beesKilledByPlayer;

	private void Start ()
    {
        InitializeBackground();
        Vector3 topLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        worldHeight = topLeftPoint.y * 2;
        Vector3 bottomRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        worldWidth = bottomRightPoint.x * 2;
        Debug.Log("Width = " + worldWidth + " Height = " + worldHeight);       
    }
	
	private void Update ()
    {
        if (bees.Count == 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        timer += Time.deltaTime;
        if (timer > generationTime)
        {
            timer = 0;
            GenerateSpiders();
            spidersPerSpawn += 0.25f;
            generationTime *= 0.95f;
        }
	}

    public void Kill(Vector2 worldMousePosition)
    {
        var destroyedBees = new List<GameObject>();
        foreach (var bee in bees)
        {
            if (((Vector2)bee.transform.position - worldMousePosition).sqrMagnitude < killRadius * killRadius)
            {
                destroyedBees.Add(bee);
            }
        }

        foreach (var destroyedBee in destroyedBees)
        {
            beesKilledByPlayer++;
            var deadBee = Instantiate(Resources.Load<GameObject>(DeadBeePath));
            deadBee.transform.position = destroyedBee.transform.position;
            BeeDied(destroyedBee);
        }

        var destroyedSpiders = new List<GameObject>();
        foreach (var spider in spiders)
        {
            if (((Vector2)spider.transform.position - worldMousePosition).sqrMagnitude < killRadius * killRadius)
            {
                destroyedSpiders.Add(spider);                
            }
        }

        foreach (var destroyedSpider in destroyedSpiders)
        {
            var deadSpider = Instantiate(Resources.Load<GameObject>(DeadSpiderPath));
            deadSpider.transform.position = destroyedSpider.transform.position;

            spiders.Remove(destroyedSpider);
            Destroy(destroyedSpider);
            killedSpiders++;
            ScoreText.text = killedSpiders.ToString();
            ScoreImage.rectTransform.sizeDelta = new Vector2(20 + 30 * Mathf.CeilToInt(Mathf.Log10(killedSpiders + 1)), ScoreImage.rectTransform.sizeDelta.y);
        }
    }

    private void HandleMouseDown()
    {
        var mousePosition = Input.mousePosition;
        Debug.Log("Mouse position = " + mousePosition);
        mousePosition.z = 0;
        var worldMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("Mouse world position = " + worldMousePosition);

        beeKiller.Kill(worldMousePosition);

       
    }

    private void InitializeBackground()
    {
        var background = Resources.Load<GameObject>(BackgroundPath);
        var result = Instantiate(background);
        result.transform.position = Vector3.forward;
        var imageSize = result.transform.GetComponent<Renderer>().bounds.size;
        var screenHeight = Camera.main.orthographicSize * 2;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = screenAspect * camHalfHeight;
        float screenWidth = 2.01f * camHalfWidth;
        var previousLocalScale = result.transform.localScale;
        result.transform.localScale = new Vector3(
            previousLocalScale.x * screenWidth / imageSize.x,
            previousLocalScale.y * screenHeight / imageSize.y,
            previousLocalScale.z);
    }

    private void GenerateFlowers()
    {
        flowers = new List<GameObject>();        
        float minDimension = Mathf.Min(worldHeight, worldWidth);

        int flowerCount = Random.Range(25, 35);
        var beehiveBounds = Beehive.GetComponent<Renderer>().bounds.size;
        float maxBeehiveBound = Mathf.Max(beehiveBounds.x, beehiveBounds.y);

        for (int i = 0; i < flowerCount; i++)
        {
            float radius = maxBeehiveBound +
                (minDimension / 2 - maxBeehiveBound) * RandomFromDistribution.RandomFromExponentialDistribution(2, RandomFromDistribution.Direction_e.Left);
            int angle = Random.Range(0, 360);
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            var flower = Instantiate(Resources.Load<GameObject>(FlowerPath));
            flower.transform.position = new Vector3(x, y, 0);
            flowers.Add(flower);
        }
    } 

    private void GenerateBees()
    {
        bees = new List<GameObject>();
        for (int i = 0; i < BeeCount; i++)
        {
            var bee = Instantiate(Resources.Load<GameObject>(BeePath));
            bee.GetComponent<BeeController>().Initialize(flowers, Beehive);
            bee.transform.position = new Vector3(Beehive.transform.position.x, Beehive.transform.position.y, -0.01f);
            bees.Add(bee);
        }
    }

    private void GenerateSpiders()
    {
        if (spiders == null)
        {
            spiders = new List<GameObject>();
        }
        for (int i = 0; i < spidersPerSpawn; i++)
        {
            var spider = Instantiate(Resources.Load<GameObject>(SpiderPath));
            spiders.Add(spider);

            bool isXFixed = worldWidth > worldHeight;
            bool usePositiveSign = Random.Range(0, 2) == 0;
            float x, y;
            if (isXFixed)
            {
                x = (usePositiveSign ? 1 : -1) * worldWidth / 2;
                y = Random.Range(0, worldHeight) - worldHeight / 2;
            }
            else
            {
                y = (usePositiveSign ? 1 : -1) * worldHeight / 2;
                x = Random.Range(0, worldWidth) - worldWidth / 2;
            }
            spider.transform.position = new Vector3(x, y, -0.01f);
        }
    }

    private void SetupBeeKiller()
    {
        killRadius = KillRadiusPercentage * Mathf.Min(worldHeight, worldWidth);
        var beeKillerSize = BeeKillerObject.transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        BeeKillerObject.transform.localScale = killRadius * BeeKillerObject.transform.localScale / beeKillerSize.x;
        beeKiller = BeeKillerObject.GetComponent<BeeKiller>();
    }

    public GameObject GetRandomBee()
    {
        if (bees.Count == 0)
        {
            return null;
        }

        return bees[Random.Range(0, bees.Count)];
    }

    public void BeeDied(GameObject bee)
    {
        bees.Remove(bee);
        Destroy(bee);
        if (bees.Count == 0)
        {
            GameOverMenuObject.SetActive(true);
            Headline.SetActive(true);
            Beehive.SetActive(false);
            GameOverMenuObject.GetComponent<Menu>().Appear(new GameObject[][] 
            {
                new GameObject[] { FinalScoreImage.gameObject },
                new GameObject[] { SacrificesImage.gameObject },
                new GameObject[] { HighscoreImage.gameObject },                
                new GameObject[] { PlayButton, SoundButton, ExitButton }
            });
            FinalScore.text = ScoreText.text;
            if (killedSpiders > 0)
            {
                FinalScoreImage.rectTransform.sizeDelta = new Vector2(20 + 30 * Mathf.CeilToInt(Mathf.Log10(killedSpiders + 1)), FinalScoreImage.rectTransform.sizeDelta.y);
            }

            ScoreImage.gameObject.SetActive(false);
            int highscore = PlayerPrefs.GetInt(PlayerPrefsKeys.Highscore);
            if (killedSpiders > highscore)
            {
                highscore = killedSpiders;
                PlayerPrefs.SetInt(PlayerPrefsKeys.Highscore, killedSpiders);
            }

            HighscoreText.text = highscore.ToString();
            if (highscore > 0)
            {
                HighscoreImage.rectTransform.sizeDelta = new Vector2(20 + 30 * Mathf.CeilToInt(Mathf.Log10(highscore + 1)), HighscoreImage.rectTransform.sizeDelta.y);
            }

            SacrificesText.text = beesKilledByPlayer.ToString();
            if (beesKilledByPlayer > 0)
            {
                SacrificesImage.rectTransform.sizeDelta = new Vector2(20 + 30 * Mathf.CeilToInt(Mathf.Log10(beesKilledByPlayer + 1)), SacrificesImage.rectTransform.sizeDelta.y);
            }

            foreach (var spider in spiders)
            {
                Destroy(spider.gameObject);
            }

            foreach (var flower in flowers)
            {
                Destroy(flower.gameObject);
            }
        }
    }

    public void Play()
    {
        SetupBeeKiller();
        GenerateFlowers();
        GenerateBees();
        GameOverMenuObject.SetActive(false);
        HighscoreImage.gameObject.SetActive(true);
        ScoreImage.gameObject.SetActive(true);
        SoundButton.SetActive(false);
        ExitButton.SetActive(false);
        Beehive.SetActive(true);
        Headline.SetActive(false);
        beesKilledByPlayer = 0;
        killedSpiders = 0;
        spiders = new List<GameObject>();
        generationTime = 2;
        spidersPerSpawn = 1;
        ScoreText.text = killedSpiders.ToString();        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
