using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndMessage : MonoBehaviour
{
	TicTacToeAI _ai;

	[SerializeField]
	private TMP_Text _playerMessage = null;


	private void Awake()
	{
		_ai = FindObjectOfType<TicTacToeAI>();
	}

	private void Start()
	{
		_ai.onPlayerWin.AddListener((win) => OnGameEnded(win));
	}


	public void OnGameEnded(int winner)
	{
		//Debug.Log("textendmessage ran");
		_playerMessage.text = winner == -1 ? "Tie" : winner == 1 ? "AI wins" : "Player wins";
	}
}
