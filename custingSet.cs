using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class custingSet : MonoBehaviourPun,IPunObservable
{
    #region 物件
    [SerializeField]
    private Text _text;
    public GameObject loadtext;
    public GameObject playernamelist;
    public GameObject Startselect;
    #endregion


    #region 用於 static 呼叫我
    public static custingSet my;
    private void Awake()
    {
        my = this;       
    }
    #endregion


    private List<Player> thisgameplayer = new List<Player>();
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)         
            StartCoroutine(checkplayer());
        
    }

    IEnumerator checkplayer()
    {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                yield return new WaitForSeconds(0.1f);
                thisgameplayer.Add(player);
                Yesname();

                if(thisgameplayer.ToArray().Length == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    photonView.RPC("Optoin", RpcTarget.All);
                    break;
                }
            }
    }
    void Yesname()
    {
        _text.text = "";
        foreach (Player player in thisgameplayer)
        {
            if (player.IsMasterClient) { 
                _text.text += "[房主]" + player.NickName + " ☑\n";}
            else
                _text.text += player.NickName + " ☑\n";
        }        
        photonView.RPC("Nname", RpcTarget.Others, _text.text);           
    }   

    [PunRPC]
    void Nname(string name)
    {
        _text.text = name;
    }
    [PunRPC]
    void Optoin()
    {
        StartCoroutine(Loadinfo());
    }


    public int displayProgress = 0;
    IEnumerator Loadinfo()
    {
        AsyncOperation op;
        op = SceneManager.LoadSceneAsync(2);
        op.allowSceneActivation = false;
                 
                while (this)
                {
                    loadtext.transform.localPosition = Vector3.Lerp(loadtext.transform.localPosition, new Vector3(0f, -298f, 0), 0.04f);

                    if(displayProgress < (op.progress * 100 + 10)) 
                    {
                        yield return new WaitForSeconds(0.03f);
                        displayProgress++;
                        loadtext.GetComponent<Text>().text = "讀取中...";
                        loadtext.GetComponent<Text>().text += "\n" + displayProgress + " %";
                        if (displayProgress == 99)
                        {
                            yield return new WaitForSeconds(1f);
                            loadtext.GetComponent<Text>().text = "即將完成!\n現在要選擇你的初始狀態";
                            yield return new WaitForSeconds(4f);
                            break;
                        }
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                touchLaisa.my.endjob();
                yield return new WaitForSeconds(0.5f);
                while (this)
                {
                    yield return new WaitForSeconds(0.01f);                    
                    if (loadtext.transform.localPosition.y <= -588f && playernamelist.transform.localPosition.x <= -1160f)
                    {
                        Startselect.transform.localPosition = Vector3.Lerp(Startselect.transform.localPosition, new Vector3(120f, Startselect.transform.localPosition.y, 0), 0.05f);
                            if (Startselect.transform.localPosition.x <= 177f)
                                    break;
                    }
                    else 
                    {
                        playernamelist.transform.localPosition = Vector3.Lerp(playernamelist.transform.localPosition, new Vector3(-1162f, 0f, 0), 0.05f);
                        loadtext.transform.localPosition = Vector3.Lerp(loadtext.transform.localPosition, new Vector3(0f, -590f, 0), 0.05f);
                    }
                }
    }
    //讀取狀態的顯示與移動
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
