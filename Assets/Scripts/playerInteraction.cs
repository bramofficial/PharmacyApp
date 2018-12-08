using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;

public class playerInteraction : MonoBehaviour {

    public GameObject game;
    public GameObject finalScreen;
    public Canvas playArea;
    //hard code these in for now
    public Button button0;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Text drugName;
    public Text score;
    public Text left;
    public Text final;

    //The state of the buttons
    bool buttonStates = true;
    //Unique list of numbers.
    int[] list;
    //the current number we are on
    int currentNumber = 0;
    //keeps our score
    int scoreValue = 0;
    //The button number that is the correct answer
    int correctButton;
    //if the user is correct
    bool correct = false;
    //triggers either when the user is right or wrong
    bool moveOn = false;
    //For randomly selecting from our drugList
    int randomDrug = 0;
    //Comes from the Main Menu
    int gameType = 0;
    //Brand, Generic
    Dictionary<string, string> drugList = new Dictionary<string, string>();
    //A dictionary of our missed drugs
    Dictionary<string, string> missedList = new Dictionary<string, string>();

	// Use this for initialization
	void Start () {

        //We will use this variable to load up different rules depending on the button clicked
        gameType = PlayerPrefs.GetInt("gameMode");
        //gameType = 0;
        if (gameType != 2)
            loadDrugList("Drug List3");
        else
            loadDrugList("Drug List");

        left.GetComponentInChildren<Text>().text = "Left: " + drugList.Count;

        list = new int[drugList.Count];

        //Debug.Log("Path: " + Application.dataPath);
        //drugList.Keys.ElementAt(0);
        Debug.Log("DRUGLIST COUNT: " + drugList.Count);

        //Debug.Log(drugList.Keys.ElementAt(11) + " || " + drugList[drugList.Keys.ElementAt(181)]);
        /* drugList.
         * 
         */
        
        //set to 0 to 200 but can change when it comes down to testing
        list = generateUniqueNumbers(0, drugList.Count);

        string numbers = "";
        foreach (var item in list)
        {
            numbers += item + " ";
        }
        Debug.Log("Unique List: " + numbers);

        

        //Sets the onClickListener to our button0Click
        button0.onClick.AddListener(button0Click);
        button1.onClick.AddListener(button1Click);
        button2.onClick.AddListener(button2Click);
        button3.onClick.AddListener(button3Click);
        button4.onClick.AddListener(button4Click);

        //initialize the first buttons 
        setUpButtons();
       
    }
	
	// Update is called once per frame
	void Update () {
   
        //Whether they got the right answer we move on and process the info
        if (moveOn) {
            //checking if we aren't hitting the end of our list
            if((currentNumber + 1) != drugList.Count)
            {
                //used to find the next index, just starts at 0, 1, 2, 3,...,199
                currentNumber++;
                left.GetComponentInChildren<Text>().text = "Left: " + (drugList.Count - currentNumber);
                //Correct so just reset vars, add some score, and keep moving
                if (correct)
                {
                    scoreValue++;
                    score.GetComponentInChildren<Text>().text = "Score: " + scoreValue;
                    correct = false;
                    moveOn = false;
                }
                else
                { //User was not correct; reset some vars and add our missed word into a list to test later
                    moveOn = false;
                }
                //This calls setupbuttons and allows for a delay to be set up
                StartCoroutine(Waiting());
            } else {
                //we are at the very end, make stuff disappear
                //Debug.Log("WE HAVE REACHED THE END OF THE LIST");
                finalScreen.SetActive(true);
                
                final.GetComponent<Text>().text = "You managed to get " + scoreValue + " correct out of " + drugList.Count;
                game.SetActive(false);
                //Debug.Log(missedList.Count);
            }

            buttonStates = true;
        }
    }

    /*
     * Handles when button 0-4 is clicked
     */
    void button0Click() {

        button0.GetComponent<Button>().interactable = false;

        //we add score if the user gets it correct
        if (correctButton == 0)
        {
            button0.GetComponentInChildren<Text>().text = "CORRECT";
            moveOn = true;
            correct = true;
        }
        else
        { //not correct but we need to move on still
            button0.GetComponentInChildren<Text>().text = "INCORRECT";
            //Debug.Log();
            //missedList.Add(drugList.Keys.ElementAt(randomDrug), drugList[drugList.Keys.ElementAt(randomDrug)]);
            moveOn = true;
            correct = false;
        }
    }

    void button1Click() {

        button1.GetComponent<Button>().interactable = false;

        if (correctButton == 1)
        {
            button1.GetComponentInChildren<Text>().text = "CORRECT";
            correct = true;
            moveOn = true;
        }
        else
        {
            button1.GetComponentInChildren<Text>().text = "INCORRECT";
            //missedList.Add(drugList.Keys.ElementAt(randomDrug), drugList[drugList.Keys.ElementAt(randomDrug)]);
            moveOn = true;
            correct = false;
        }
        
    }

    void button2Click() {

        button2.GetComponent<Button>().interactable = false;

        if (correctButton == 2)
        {
            button2.GetComponentInChildren<Text>().text = "CORRECT";
            correct = true;
            moveOn = true;
        }
        else
        {
            button2.GetComponentInChildren<Text>().text = "INCORRECT";
            //missedList.Add(drugList.Keys.ElementAt(randomDrug), drugList[drugList.Keys.ElementAt(randomDrug)]);
            moveOn = true;
            correct = false;
        }
       
    }

    void button3Click() {

        button3.GetComponent<Button>().interactable = false;

        if (correctButton == 3)
        {
            button3.GetComponentInChildren<Text>().text = "CORRECT";
            correct = true;
            moveOn = true;
        }
        else
        {
            button3.GetComponentInChildren<Text>().text = "INCORRECT";
            //missedList.Add(drugList.Keys.ElementAt(randomDrug), drugList[drugList.Keys.ElementAt(randomDrug)]);
            moveOn = true;
            correct = false;
        }
    }

    void button4Click() {

        button4.GetComponent<Button>().interactable = false;

        if (correctButton == 4)
        {
            button4.GetComponentInChildren<Text>().text = "CORRECT";
            correct = true;
            moveOn = true;
        }
        else
        {
            button4.GetComponentInChildren<Text>().text = "INCORRECT";
            //missedList.Add(drugList.Keys.ElementAt(randomDrug), drugList[drugList.Keys.ElementAt(randomDrug)]);
            moveOn = true;
            correct = false;
        }
    }

    //Load the drug list from a file, called only once when we start
    void loadDrugList(string fileName){
        try
        {
            TextAsset drugText = Resources.Load<TextAsset>("Files/" + fileName);

            if (drugText != null)
            {
                using(StreamReader sr = new StreamReader(new MemoryStream(drugText.bytes)))
                {
                    string line;
                    do
                    {
                        line = sr.ReadLine();
                        Debug.Log("LINE: " + line);
                        if(line != null || line != "EOF")
                        {
                            string[] lineParts = line.Split('=');

                            drugList.Add(lineParts[0], lineParts[1]);
                        }
                    } while (line != null);
                }
            }
        }
        catch (System.Exception e)
        {
            
        }

    }

    /*
     * This function is to set up the canvas: set the text to the drug we want to match
     * and one button to the correct match and the other buttons to random drugs from the left overs (no dupes)
     */
    void setUpButtons() {
        //randomDrug = Random.Range(0, 21);
        Debug.Log("current: " + currentNumber);
        randomDrug = list[currentNumber];
        correctButton = Random.Range(0, 5);
        
        /*Load by game type
         * 0 = Brand to generic
         * 1 = Generic to brand
         */
        switch (gameType)
        {
            
            case 0:
                    //Set the text to the correct drug with the correct game type
                    Debug.Log("randomDrug: " + randomDrug + " || correctNumber: " + correctButton);
                    drugName.GetComponentInChildren<Text>().text = "Brand: " + drugList.Keys.ElementAt(randomDrug);
                    brandToGeneric(correctButton);
                    break;

            case 1:
                    Debug.Log("randomDrug: " + randomDrug + " || drug number: " + drugList[drugList.Keys.ElementAt(randomDrug)]);
                    drugName.GetComponentInChildren<Text>().text = "Generic: " + drugList[drugList.Keys.ElementAt(randomDrug)];
                    genericToBrand(correctButton);
                    break;

            case 2:
                    drugName.GetComponentInChildren<Text>().text = "Brand: " + drugList.Keys.ElementAt(randomDrug);
                    brandToGeneric(correctButton);
                    break;

        }
    }

    //generates a list of numbers that are unique from start to finsih, so we can move through the list
    int [] generateUniqueNumbers(int start, int finish)
    {
        HashSet<int> uniqueList = new HashSet<int>();
        do
        {
            uniqueList.Add(Random.Range(start, finish));
        } while (uniqueList.Count != finish);

        return uniqueList.ToArray();
    }

    int [] getRandomNumbers(int correct)
    {
        Debug.Log("Correct number: " + correct);
        HashSet<int> randomNumbers = new HashSet<int>();
        randomNumbers.Add(correct);
        int number;
        do
        {
            //Debug.Log("IN OUR DO WHILE");
            number = Random.Range(0, drugList.Count); 
            randomNumbers.Add(number);
        } while (randomNumbers.Count != 5); //5

        //Testing purposes
        string numbers = "";
        foreach(var item in randomNumbers.ToArray())
        {
            numbers += item + " ";
        }
        Debug.Log("The array: " + numbers);
        return randomNumbers.ToArray();
    }

    /*
     * Handles the matching of brand to the generic 
     */
    void brandToGeneric(int number)
    {
        Debug.Log("IN BRAND TO GENERIC: NUMBER = " + number);
        //Randomly assign a button to the correct match
        switch (number)
        {
            case 0:
                //Set the correct button to the correct drug
                button0.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomDrug)];
                //Get the random values of the next buttons
                int[] randomNumber = getRandomNumbers(randomDrug);

                //Set these to random values that aren't our randomDrug
                button1.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber[1])];
                button2.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber[2])];
                button3.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber[3])];
                button4.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber[4])];
                break;
            case 1:
                button1.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomDrug)];
                //Get the random values of the next buttons
                int[] randomNumber1 = getRandomNumbers(randomDrug);

                //Set these to random values that aren't our randomDrug
                button0.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber1[1])];
                button2.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber1[2])];
                button3.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber1[3])];
                button4.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber1[4])];
                break;
            case 2:
                button2.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomDrug)];
                //Get the random values of the next buttons
                int[] randomNumber2 = getRandomNumbers(randomDrug);

                //Set these to random values that aren't our randomDrug
                button0.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber2[1])];
                button1.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber2[2])];
                button3.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber2[3])];
                button4.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber2[4])];
                break;
            case 3:
                button3.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomDrug)];
                //Get the random values of the next buttons
                int[] randomNumber3 = getRandomNumbers(randomDrug);

                button0.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber3[1])];
                button1.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber3[2])];
                button2.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber3[3])];
                button4.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber3[4])];
                break;
            case 4:
                button4.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomDrug)];
                //Get the random values of the next buttons
                int[] randomNumber4 = getRandomNumbers(randomDrug);

                button0.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber4[1])];
                button1.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber4[2])];
                button2.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber4[3])];
                button3.GetComponentInChildren<Text>().text = drugList[drugList.Keys.ElementAt(randomNumber4[4])];
                break;
        }
    }

    void genericToBrand(int number)
    {
        //Randomly assign a button to the correct match
        switch (number)
        {
            case 0:
                //Set the correct button to the correct drug
                button0.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomDrug);
                //Get the random values of the next buttons
                int[] randomNumber = getRandomNumbers(randomDrug);

                //Set these to random values that aren't our randomDrug
                button1.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber[1]);
                button2.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber[2]);
                button3.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber[3]);
                button4.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber[4]); //Error
                break;
            case 1:
                button1.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomDrug);
                //Get the random values of the next buttons
                int[] randomNumber1 = getRandomNumbers(randomDrug);

                //Set these to random values that aren't our randomDrug
                button0.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber1[1]); //Error still
                button2.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber1[2]);
                button3.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber1[3]);
                button4.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber1[4]);
                break;
            case 2:
                button2.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomDrug);
                //Get the random values of the next buttons
                int[] randomNumber2 = getRandomNumbers(randomDrug);

                //Set these to random values that aren't our randomDrug
                button0.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber2[1]);
                button1.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber2[2]);
                button3.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber2[3]);
                button4.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber2[4]);
                break;
            case 3:
                button3.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomDrug);
                //Get the random values of the next buttons
                int[] randomNumber3 = getRandomNumbers(randomDrug);

                button0.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber3[1]); //Error
                button1.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber3[2]);
                button2.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber3[3]);
                button4.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber3[4]);
                break;
            case 4:
                button4.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomDrug);
                //Get the random values of the next buttons
                int[] randomNumber4 = getRandomNumbers(randomDrug);

                button0.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber4[1]);
                button1.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber4[2]);
                button2.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber4[3]);
                button3.GetComponentInChildren<Text>().text = drugList.Keys.ElementAt(randomNumber4[4]);
                break;
        }
    }

    //Restarts the game when either the button is pressed or if we reach 0 left and the restart button is clicked
    public void Restart() {

        Debug.Log("There are " + (drugList.Count - currentNumber + 1) + " left" );
        //the number left (durugList.Count - currentNumber) is equal to 0
        if((drugList.Count - (currentNumber + 1)) == 0)
        {
            Debug.Log("THIS IS TRUE, 0 LEFT");
            SceneManager.LoadScene(2);

        }
        //Just the button was pressed, so restart the scene normally 
        else
        {
            SceneManager.LoadScene(2);   
        }
    }

    //This function allows us to delay the game then we call the setupbuttons
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(3);

        setUpButtons();

        button0.GetComponent<Button>().interactable = true;
        button1.GetComponent<Button>().interactable = true;
        button2.GetComponent<Button>().interactable = true;
        button3.GetComponent<Button>().interactable = true;
        button4.GetComponent<Button>().interactable = true;
    }
}
