using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class web : MonoBehaviour
{
    [System.Serializable]
    public struct webDataStructure
    {
        [System.Serializable]
        public struct registration
        {
            public string name;
            public int point;
        }
        public List<registration> registrations;
    }

    public webDataStructure data;

    public Transform table;
    public Transform newScore;
    public Transform highscoreTable;
    public GameObject registrationObject;
    int numberRegistration = 10;
    private int myPoint;

    public TMPro.TMP_InputField myName;

    public Principal principal;

    [ContextMenu("Ler")]
    public void Read(System.Action actionToFinish)
    {
        StartCoroutine(CoroutineRead(actionToFinish));
    }

    private IEnumerator CoroutineRead(System.Action actionToFinish)
    {
        UnityWebRequest web = UnityWebRequest.Get("https://binigames.com/games/timber/HighscoreFolder/HighscoreTable.txt");
        yield return web.SendWebRequest();
        //esperamos ele voltar...
        //voltou
        if (!web.isNetworkError && !web.isHttpError) // tudo ok
        {
            data = JsonUtility.FromJson<webDataStructure>(web.downloadHandler.text);
            actionToFinish();
        }
        else
        {
            Debug.LogWarning("Problema");
        }
    }

    [ContextMenu("Escrever")]
    public void Write()
    {
        StartCoroutine(CoroutineWrite());
    }

    private IEnumerator CoroutineWrite()
    {
        WWWForm form = new WWWForm();
        form.AddField("arquivo", "HighscoreTable.txt");
        form.AddField("texto", JsonUtility.ToJson(data));

        UnityWebRequest web = UnityWebRequest.Post(
            "https://binigames.com/games/timber/HighscoreFolder/escrevi.php",
            form
            );
        yield return web.SendWebRequest();
        //esperamos ele voltar...
        //voltou
        if (!web.isNetworkError && !web.isHttpError) // tudo ok
        {
            Debug.Log(web.downloadHandler.text);
        }
        else
        {
            Debug.LogWarning("Problema");
        }
    }

    [ContextMenu("Criar tabela")]
    void CreateTable()
    {
        for (int i = 0; i < numberRegistration; i++)
        {
            GameObject inst = Instantiate(registrationObject, table);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -35f);
            inst.name.ToString();
        }
    }

    [ContextMenu("Passar dadoa a tabela")]
    void PassDataToTable()
    {
        for (int i = 0; i < numberRegistration; i++)
        {
            table.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = data.registrations[i].name;
            table.GetChild(i).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = data.registrations[i].point.ToString();
        }
    }

    [ContextMenu("Checar se corresponde ")]
    public void CheckIfCorrespondsToNewScore(int score)
    {
        myPoint = score;

        if (myPoint > data.registrations[numberRegistration - 1].point)
        {
            highscoreTable.gameObject.SetActive(false);
            table.gameObject.SetActive(false);
            newScore.gameObject.SetActive(true);
        }
        else
        {
            highscoreTable.gameObject.SetActive(true);
            table.gameObject.SetActive(true);
            newScore.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Inserir registro")]
    void InsertNewRegistration()
    {
        //posicion insert
        for (int i = 0; i < numberRegistration; i++)
        {
            if(myPoint > data.registrations[i].point)
            {
                data.registrations.Insert(i, new webDataStructure.registration()
                {
                    name = myName.text,
                    point = myPoint
                });

                break;
            }
        }
    }

    void Start()
    {
        Read(CreateTablePassDataAndCheck);
    }

    public void CreateTablePassDataAndCheck()
    {
        CreateTable();
        PassDataToTable();
        //CheckIfCorrespondsToNewScore(principal.score);
    }

    public void InputFinish()
    {
        newScore.gameObject.SetActive(false);
        highscoreTable.gameObject.SetActive(true);
        table.gameObject.SetActive(true);
        Read(InsertAndWrite);
    }

    void InsertAndWrite()
    {
        InsertNewRegistration();
        Write();
        PassDataToTable();
    }

}
