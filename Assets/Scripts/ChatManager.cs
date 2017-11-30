﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

using UnityEngine.UI;
using System;

public class ChatManager : MonoBehaviour {


    private RectTransform chatContent;
    private Text chatLine;
    private InputField inputField;
    private ScrollRect chatScroll;
    private NetworkConnectionP2P net;
    // Use this for initialization
    void Start ()
    {
        net = gameObject.GetComponent<NetworkConnectionP2P>();
        chatContent = GameObject.Find("ChatContent").GetComponent<RectTransform>();
        chatLine = GameObject.Find("Chat Line").GetComponent<Text>();
        chatLine.gameObject.SetActive(false);
        inputField = GameObject.Find("ChatInput").GetComponent<InputField>(); ;
        chatScroll = GameObject.Find("Scroll View").GetComponent<ScrollRect>(); ;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AppendNewText(string text)
    {
        var newChatLine = (Instantiate(chatLine.gameObject)).GetComponent<Text>();
        newChatLine.text = text;
        newChatLine.gameObject.SetActive(true);
        newChatLine.rectTransform.SetParent(chatContent);

        // If you set position to 0,  there is weird unity behavior that because although this is after 
        // adding the new line, it hasn't actually been instantiated yet, so it is always 1 line up from the last
        // and if you set to 0 with downward velocity, velocity doesn't work unless its not 0? 
        // so dumb, need a better way to move to bottom of scroll
        chatScroll.verticalNormalizedPosition = 0.1f;
        chatScroll.velocity = new Vector2(0, 500f);
    }
    public void SubmitChatInput(InputField input)
    {
        if (input.text.Length > 0)
        {
            var text = "Player " + net.this_player_id + ": " + input.text;
            AppendNewText(text);

            //reset input
            input.text = "";

            // regain focus after hitting enter
            input.ActivateInputField();
            input.Select();

            // send out to other players
            net.SendData(text, 0);
        }

    }

}