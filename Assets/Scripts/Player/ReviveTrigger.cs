using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReviveTrigger : MonoBehaviour
{
    public RectTransform revivePanel;
    public Button reviveButton;

    private CharacterState characterState;
    [SerializeField]
    private PlayerController playerToRevive;


    
    void OnEnable()
    {
        reviveButton.onClick.AddListener(TaskOnClick);
    }

    void OnDisable()
    {
        reviveButton.onClick.RemoveListener(TaskOnClick);
    }

    void Start()
    {
        characterState = GetComponentInParent<CharacterState>();
        revivePanel.gameObject.SetActive(false);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (characterState.status != CharacterStatus.ALIVE)
            {
                // DEBUG
                var otherId = other.GetComponent<PlayerController>().tmpNetworkId;
                var myId = gameObject.GetComponentInParent<PlayerController>().tmpNetworkId;

                // When Player2 ENTERS Player1 revive zone, we get Player2's UI revive panel
                // so we can show it on Player2 screen...
                var otherPlayerPanel = other.GetComponentInChildren<ReviveTrigger>();
                otherPlayerPanel.revivePanel.gameObject.SetActive(true);

                // Also, when Player2 ENTERS the revive zone, we want to get Player1 gameObject...
                other.GetComponentInChildren<ReviveTrigger>().playerToRevive = this.gameObject.GetComponentInParent<PlayerController>();

                DevLog.Log("ReviveTrigger", "Player id <" + otherId + "> entered vicinity of <" + myId + ">");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (characterState.status != CharacterStatus.ALIVE)
            {
                // DEBUG
                var otherId = other.GetComponent<PlayerController>().tmpNetworkId;
                var myId = gameObject.GetComponentInParent<PlayerController>().tmpNetworkId;

                // When Player2 EXITS Player1 revive zone, we get Player2's UI revive panel
                // so we can disable it on Player2 screen...
                var otherPlayerPanel = other.GetComponentInChildren<ReviveTrigger>();
                otherPlayerPanel.revivePanel.gameObject.SetActive(false);

                // Also, when Player2 EXITS the revive zone, we cannot revive anyone...
                other.GetComponentInChildren<ReviveTrigger>().playerToRevive = null;

                DevLog.Log("ReviveTrigger", "Player id <" + otherId + "> exited vicinity of <" + myId + ">");
            }
        }
    }


    void TaskOnClick()
    {
        var whoseId = gameObject.GetComponentInParent<PlayerController>().tmpNetworkId;

        //Output this to console when Button1 or Button3 is clicked
        DevLog.Log("ReviveTrigger", "Player id <" + whoseId + "> has clicked the button!");

        if (this.playerToRevive != null)
        {
            DevLog.Log("ReviveTrigger", "Player id <" + whoseId + "> attempting to revive <" + this.playerToRevive.tmpNetworkId + ">");
            this.playerToRevive.CmdRevive();
            this.revivePanel.gameObject.SetActive(false);
            this.GetComponentInChildren<ReviveTrigger>().playerToRevive = null;
        }
    }
}
