# Monitoring Task

## Prerequisites

- Visual Studio (or Visual Studio Code)
- .NET Framework
- SQL Server LocalDB
- Mail server local like HmailServer

## Installation

1. Clone this repository to your local machine:
   https://github.com/didiianto/MonotoringTask.git
   
2. Open the solution in Visual Studio.

3. Build the solution to restore NuGet packages.
	`Update-Package -reinstall` in Package Manager Console
4. In the `web.config` file of the project, configure the email settings under the `<appSettings>` section. Replace the value with your email settings

   
## Usage
- Press F5 or run the application to start the local development server.

- Access the application in your web browser at http://localhost:port (replace port with the actual port number).

## Login Access
- User : user1@local.com
- Password : Abc1223++