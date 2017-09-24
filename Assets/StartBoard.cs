using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartBoard : MonoBehaviour
{
    public BoardInfo Info;
    public Game Game;
    public Menu Menu;

    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(start);
    }

    private void start()
    {
        G.BoardInfo = Info;
        Game.gameObject.SetActive(true);
        Menu.gameObject.SetActive(false);
    }
}