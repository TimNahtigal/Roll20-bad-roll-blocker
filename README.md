# Roll20 bad roll blocker

With this tool you can block any unwanted rolls effectively making you roll any number you want on roleplaying website [roll20](https://roll20.net/). 
It exploits the fact that your rolls are made on your computer.

Was already reported to roll20 staff. 

![Screenshot](https://media.discordapp.net/attachments/589179848484716585/904833416313307146/unknown.png)

*P.S. Don't use this tool maliciously. It really degrades the quality of the game, but you are a big boy so you won't use it to ruin the fun of other players, right? :P*
---
## How to setup

1. First you compile and run this program. 
2. Second you under 'settings->internet->proxy->manual proxy setup' you set address as "https://localhost/" and port as "8080"
3. You join Roll20 game

## How to use

- Entering a number sets a minimal roll, anything less than that number wouldn't show in the chat
- Entering "s" sets program into proficiency mode. In this mode you can roll stuff that use your prof. modifier i.e. skills
- Entering "m" sets program into misc mode for all the rolls that don't use your prof. modifier i.e. initiative
- clear clears the screen
- "color 'number' 'number'" sets the color of the terminal, you can see the colors with command "chelp". First number sets the text color and second sets background color
- "x" turns the program on/off. Program acts as a proxy so if you set the proxy and then turn off program manually you lose connection :|
