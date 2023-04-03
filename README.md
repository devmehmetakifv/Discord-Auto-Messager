# Discord Auto Messager

A Discord bot with GUI that sends different messages to different channels with different time intervals using HTTP POST requests, developed in C#.

# Description

Comes with KeyMessagerClient class that you can use to create your own Discord bots.

Create new instance of the class and use the constructor to pass in all neccessary data.

```
KeyMessagerClient client = new KeyMessagerClient(fileContentsToSend,channelNums,timeIntervals,authorizationCode);
```

Use asynchronous InitiateProcess() function to begin sending messages.

```
await client.InitiateProcess();
```

Be sure to modify links.json with your own custom links to send your messages to correct channels.

You can find 'authorizationCode' by following these steps:

1- Go to any channel on Discord
2- Hit F12
3- Select 'Network'
4- Type something on channel (don't send).
5- You will notice 'typing' statement appear on the console. Click on it.
6- Scroll down untill you see 'authorization-code'.


# Attention!

Bot is HIGHLY UNSTABLE and may not work CORRECTLY! There are a few logical mistakes in client code that might effect the program's functionality. Be prepared for them.

