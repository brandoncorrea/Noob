# Noob.API
Noob Discord Bot

#### Use this link to add it to your server!

https://discord.com/api/oauth2/authorize?client_id=1027797501660643368&permissions=0&scope=bot%20applications.commands

#### Join our Discord Server

https://discord.gg/VT3YqXSqf

## Getting Started

Clone this repo and initialize some files. You will need your Discord Bot's Token.

````
git clone https://github.com/brandoncorrea/Noob.git
cd Noob/Noob.App
touch migrations.txt
mkdir db
echo "DISCORD_TOKEN" > discord.token
````

## Running as a Service

Copy this text to `/etc/systemd/system/noob.service` with the app's directory and your system user.

````service
[Unit]
Description=Noob Service
StartLimitIntervalSec=30
StartLimitBurst=5

[Service]
Type=simple
Restart=always
RestartSec=1
WorkingDirectory=/Path/To/Noob/Project/Noob/Noob.App
ExecStart=dotnet run --configuration Release
User=YOUR_SYSTEM_USER
````

Reload:
  - `systemctl daemon-reload`

Run:
  - `sudo systemctl start noob`

Verify:
  - `systemctl status noob`

## Run

`dotnet run --configuration Release`

## Test

`dotnet test`

## Deploy

### Create a Linux environment
  - Recommended Specs:
    - 1GB RAM
    - 8GB SSD
    - Ubuntu LTS

### SSH to Linux

### Install Packages

````shell
sudo apt-get update
sudo apt install -y dotnet-sdk-6.0 aspnetcore-runtime-6.0 sqlite3
````

### Clone this repository

````shell
git clone https://github.com/brandoncorrea/Noob.git
````

### Build the Project

````shell
cd Noob/Noob.App
dotnet build --configuration Release
````

### Create your Discord Token

````shell
echo "TOKEN" >> discord.token
````

### Copy Down the Database

````shell
scp -i ~/.ssh/ssh-key.cer ubuntu@remote:/home/ubuntu/Noob/Noob.App/db/noob.db noob.db
````

### Initialize the Database

````shell
mkdir db
````

### Initialize Migrations History

````shell
touch migrations.txt
````

### Create the Noob Service

````service
[Unit]
Description=Noob Service
StartLimitIntervalSec=30
StartLimitBurst=5

[Service]
Type=simple
Restart=always
RestartSec=1
WorkingDirectory=/home/ubuntu/Noob/Noob.App
ExecStart=dotnet run --configuration Release
User=ubuntu
````

````shell
sudo nano /etc/systemd/system/noob.service
sudo systemctl daemon-reload
````

### Start the Noob Service

````shell
sudo systemctl start noob
````
