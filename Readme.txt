A game with the background of pirate era in which players write a script to control a fleet to battle against another script-controlled fleet. The game is used as the platform for entrance competition for Microsoft China’s “Beauty of Programming 2012” Coding Competition.
一个基于海盗时代背景的游戏，它被用作微软“编程之美2012全国挑战赛”的预选赛平台。
This was a teamwork, I designed the rule and coded for the game part while, for example, the lijiancheng0614 take care of the AI API and connection part. The artwork is from our MSTC(Microsoft Student Technology Club). (See Credits.png for detailed staff.)
这是一个团队工作，我负责了游戏规则的设计并编写了游戏部分，然后例如lijiancheng0614负责AI接口的设计与实现，美术工作是由北航MSTC的成员完成。（详细成员参见Credits.png）

Mainly I wrote all the game part except the MSTCOS.Network and the StartMenu. 

AI api are not my work, and the AI SDK of Ocean Scream is not here.
Here I provide two AI for test, just run them and then run the compiled Ocean Scream and you can play. 
These test AIs are from the “Beauty of Programming 2012” disscussion group, and they are not my work.
AI的开发包并未包含在这里。
但是提供了几个用于测试的AI，它们来自于“编程之美2012”的讨论群。

Ported to Visual Studio 2013 and XNA 4.0 refresh(https://msxna.codeplex.com/releases/view/117230).



Rule:
1. Destroy all of the enemy fleet to win. Upon time limit, the fleet with more ships wins.
2. The more island you possess, the faster your fleet's hitpoint restore
3. Critical hit if you hit directly in the front or the back of a ship
4. Massive damage upun ram impact.
规则:
1. 全灭或是时间到后自动决定胜负。
2. 占的岛越多全队回血越快。
3. 击中船头和船尾有致命一击。
4. 冲撞时船舷被撞者受到大量伤害。