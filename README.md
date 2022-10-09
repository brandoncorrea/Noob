# Noob.API
Noob Discord Bot

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
