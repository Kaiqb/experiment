# Deploy your bot to Azure
After you have created your bot and tested it locally, you can deploy it to Azure to make it accessible from anywhere. Deploying your bot to Azure will involve paying for the services you use.

## Intro to running bots on Azure

## Set up Azure CLI resources
* Installing latest version of Azure cli.
  - .MSI file available with lab materials.
  - removing botservice if installed

## Publishing you weather bot to Azure
* Describe V4.4 and above Deployment Templates
  - template-with-new-rg.json
    - commands to create and use an new resource group.
  - template-with-preexisting-rg.json
    - commands to us an existing resource group.
* How to retrieve or create existing IIS/Kudo files.
* How to create a .zip file for Azure deployment
  - file level required by Azure.
  - directory location for running AZ Cli Deployment command. 
  
## Testing you weather bot to Azure
* test your published app using Azure Web Chat.
* Access your published app on Azure using the Bot Framework Emulator.
  - How to add UserName and Password to Access bot in Azure.
  
Online documentation reference: [Deploy Your Bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-deploy-az-cli?view=azure-bot-service-4.0&tabs=erg)
