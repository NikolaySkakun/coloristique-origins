using UnityEngine;
using System.Collections;

public class MessageController
{
	static int currentLevel = 0, lastSituation = -1, oneSituationCount = 0;

	static string[,] messages = 
	new string[,] {
		{"empty"},
		{"this is wrong decision", "maybe there is another way out?", "maybe you are too stupid to understand rules"},
		{"Try it again",
			
			"Not quick enough",
				
			"This is not so complicated: be faster"}


	};

	static public string GetMessage(int level, int situation)
	{
		string message = "";
		int pointer = level == currentLevel ? (lastSituation == situation ? (++oneSituationCount) : oneSituationCount = 0) : oneSituationCount = 0 ;

		switch(level)
		{

		case 2:
			switch(situation)
			{
			case 0:
				switch(pointer)
				{
				case 0:
				{
					message = "this is wrong decision";
				} break;
				case 1:
				{
					message = "maybe there is another way out?";
				} break;
				default: case 2:
				{
					message = "maybe you are too stupid to understand rules";
				} break;
					
				} break;
			default: break;
			} break;

		case 3:
		switch(situation)
		{
			case 0:
				switch(pointer)
				{
					case 0:
					{
						message = "try it again";
					} break;
					case 1:
					{
						message = "not quick enough";
					} break;
					default: case 2:
					{
						message = "be faster";
					} break;
					
				} break;
			case 1:
				switch(pointer)
				{
				case 0:
				{
					message = "good guess. but no";
				} break;
				case 1:
				{
					message = "this is the way. but not now";
				} break;
				default: case 2:
				{
					message = "wrong order of your actions";
				} break;
					
				} break;

			case 2:
				switch(pointer)
				{
				case 0:
				{
					message = "no rush";
				} break;
				case 1:
				{
					message = "quick. but not smart";
				} break;
				default: case 2:
				{
					message = "maybe you should try the other side";
				} break;
					
				} break;
			default: break;
		} break;

		case 4:
			switch(situation)
			{
			case 0:
				switch(pointer)
				{
				case 0:
				{
					message = "think about what are you doing";
				} break;
				case 1:
				{
					message = "maybe something need to stay on its place";
				} break;
				default: case 2:
				{
					message = "ok. you will never make it";
				} break;
					
				} break;
			case 1:
				switch(pointer)
				{
				case 0:
				{
					message = "no need in so much spheres down there";
				} break;
				case 1:
				{
					message = "not anough spheres right here";
				} break;
				case 2:
				{
					message = "you need balls";
				} break;
				case 3:
				{
					message = "ok. you have it. you have balls of steel";
				} break;
				default: { message = "no need in so much spheres down there";
				} break;
				} break;
				
			case 2:
				switch(pointer)
				{
				case 0:
				{
					message = "you just can not figure it out";
				} break;
				case 1:
				{
					message = "yoo smart. this is just not so complicated";
				} break;
				default: case 2:
				{
					message = "right now you need both of your balls";
				} break;
					
				} break;
			default: break;
			} break;


		case 5:
			switch(situation)
			{
			case 0:
				switch(pointer)
				{
					case 0:
					{
					message = "you forget something";
					} break;
				default: case 1:
					{
					message = "you forget your sphere";
					} break;
					
				} break;
			default: break;
			} break;

		default: break;
		}

		currentLevel = level;
		lastSituation = situation;

		return message;
	}
}
