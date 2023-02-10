using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class RegisterAndLogin : MonoBehaviour
{

    [SerializeField]
    private string ServerApiURL;

    public string Token { get; set; }
    public string userName { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Token = PlayerPrefs.GetString("token");
        userName = PlayerPrefs.GetString("userName");

        if(string.IsNullOrEmpty(Token))
        {
            Debug.Log("No hay token");
        }
        else
        {
            Debug.Log(Token);
            Debug.Log(userName);
            StartCoroutine(GetPerfil());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ingresar()
    {

        User user = new User();
        user.userName = GameObject.Find("InputUserName").GetComponent<InputField>().text;
        user.password = GameObject.Find("InputPassword").GetComponent<InputField>().text;
        string postData = JsonUtility.ToJson(user);
        StartCoroutine(LogIn(postData));
    }
    public void Registrar()
    {

        User user = new User();
        user.userName = GameObject.Find("InputUserName").GetComponent<InputField>().text;
        user.password = GameObject.Find("InputPassword").GetComponent<InputField>().text;
        string postData = JsonUtility.ToJson(user);
        StartCoroutine(Register(postData));
    }

    IEnumerator GetPerfil()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerApiURL + "api/usuarios/"+ userName);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                AuthJsonData jasondata = JsonUtility.FromJson<AuthJsonData>(www.downloadHandler.text);
                Debug.Log(jasondata.usuario._id + "esta registrado");

            }
            else
            {
                string mensaje = "Status" + www.responseCode;
                mensaje += "\ncontent-type" + www.GetResponseHeader("Content-type");

                mensaje += "nError :" + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    public void tabla()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerApiURL + "api/usuarios/");
       UserList jasondata = JsonUtility.FromJson<UserList>(www.downloadHandler.text);
        var listaOrdenada = jasondata.lista.OrderByDescending(U => U._id).ToList<User>();

        foreach(User user in listaOrdenada)
        {
            Debug.Log(user._id.ToString());
            Debug.Log(user.userName);
            Debug.Log(user.score.ToString());
        }
      
    }
    IEnumerator Register(string Postdata)
    {
        UnityWebRequest www = UnityWebRequest.Put(ServerApiURL + "api/usuarios/", Postdata);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                AuthJsonData jasondata= JsonUtility.FromJson<AuthJsonData>(www.downloadHandler.text);
                Debug.Log(jasondata.usuario._id+ "se ha registrado");

            }
            else
            {
                string mensaje = "Status" + www.responseCode;
                mensaje += "\ncontent-type" + www.GetResponseHeader("Content-type");

                mensaje += "nError :" + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    IEnumerator LogIn(string Postdata)
    {
        UnityWebRequest www = UnityWebRequest.Put(ServerApiURL + "api/auth/login", Postdata);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                AuthJsonData jasondata = JsonUtility.FromJson<AuthJsonData>(www.downloadHandler.text);
                Debug.Log(jasondata.usuario._id + "está registrado");

                Token = jasondata.token;
                userName = jasondata.usuario.userName;

                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("userName", userName);

            }
            else
            {
                string mensaje = "Status" + www.responseCode;
                mensaje += "\ncontent-type" + www.GetResponseHeader("Content-type");

                mensaje += "nError :" + www.error;
                Debug.Log(mensaje);
            }
        }
    }


}

public class User
{
    public int _id;
    public string userName;
    public string password;
    public int score;
}

public class AuthJsonData
{
    public User usuario;
    public string token;
}

    public class UserList
    {
        public List<User> lista ;
    }