# Lab 6, Deploy and run weather bot in Azure

In this lab we will walk through the bot deployment instructions detailed in the online documentation [Deploy your bot](https://docs.microsoft.com/azure/bot-service/bot-builder-deploy-az-cli?view=azure-bot-service-4.0&tabs=newrg). This deployment process uses an ARM template to provision required resources for the bot in Azure using Azure CLI. We will use this document to expand on particular details as they are needed.

## Open the resources to follow these steps
* Right click the link provided above and "Open in new Window" to have access to the step-by-step instructions.
* Make sure you download the [latest Azure CLI](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest)
* Open a command prompt and navigate to the folder containing Lab 5's DispatchWeatherBot.csproj file.
 
## Follow the online instructions
Now begin following the "Deploy you bot" instructions beginning at section **Login to Azure**
* The _az login_ command upon completion will provide you with a list of your available subscriptions.
* For step **Set the subscription** use the ID of the Azure Subscription you used to create your QnA Maker Service link.
* for set **Create an App registration** be sure to save locally the Password you create. This will be used later as your bot's _appSecret_ parameter.

## Choose a new or existing Azure resource group
For step **Create Azure resources** you will choose between using an existing Azure resource group, or creating a new Azure resource group. If you previously created a project resource group as part of Lab 4, when creating a QnA service, it is easy to simply reuse this resource group again. 

The Lab 5 sample code provides two ARM template files for you to choose between. Both are located inside of the "DeploymentTemplates" folder:
* template-with-new-rg.json - Choose this template to have Azure create a new resource group as part of your bot deployment.
* template-with-preexisting-rg.json - Choose this template to provide Azure with the name of an existing resource group to use for your bot deployment.

## Instructions continued
* for step **Retrieve or create necessary IIS/Kudu files** when running this command within the directory folder containing your .csproj file, the parameters provided will be:
  - --code-dir "."
  - --proj-file-path "./DispatchWeatherBot.csproj"
  
## Zip up files manually














