# Discord Auto Messager

A Discord bot with GUI that sends different messages to different channels with different time intervals using HTTP POST requests, developed in C#.

# Description

Comes with KeyMessagerClient class that you can use to create your own Discord bots.

Create new instance of the class and use the constructor to pass in all neccessary data.

'''
KeyMessagerClient client = new KeyMessagerClient(fileContentToSend,channelNums,timeIntervals,authorizationCode);
'''

Use asynchronous InitiateProcess() function to begin sending messages.

'''
await client.InitiateProcess();
'''

# Attention!
