# Cookiecutter for AWS SAM and .NET

A Cookiecutter template to create a Serverless App based on Serverless Application Model (SAM) and .NET Core 2.1

> It is important to note that you should not try to `git clone` this project but use `sam` CLI instead as ``{{cookiecutter.project_slug}}`` will be rendered based on your input and therefore all variables and files will be rendered properly.

## Requirements

Install `aws-sam-cli` as described [in the documentation](https://hosting-and-deployment.office.coolblue.eu/#/aws/lambda/README?id=aws-sam-cli).

## Usage

Generate a new SAM based Serverless App: `sam init --location gh:coolblue-development/cookiecutter-dotnet-sam`. 

You'll be prompted a few questions to help this cookiecutter template to scaffold this project and after its completed you should see a new folder at your current path with the name of the project you gave as input.

## Options

Option | Description
------------------------------------------------- | ---------------------------------------------------------------------------------
`project_name` | Name of the project
`project_short_description` | A short description of the project
`project_app_group` | The AppGroup for the project
`include_apigw` | Includes sample code for API Gateway Proxy integration for Lambda and a Catch All method in SAM as a starting point
