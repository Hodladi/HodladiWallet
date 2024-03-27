First of all you will need Phoneix Server running.
Here is some small guidance to set it up proper!
This miniguide asumes you setup Phoneix Server in /home/YourUserName/

First download latest version of Phoneix Server
```wget https://github.com/ACINQ/phoenixd/releases/download/v0.1.2/phoenix-0.1.2-linux-x64.zip```
( Doublecheck that the version is the latest [Phoenix Server](https://phoenix.acinq.co/server) )

Unzip the file
```unzip -j phoenix-0.1.2-linux-x64.zip```

Run Phoneix Server once so all files get created
```./phoneixd```

Then stop Phoneix Server by pressing CTRL + C in the Terminal window running it

Locate and open phoneix.conf
```nano /home/YourUserName/.phoneix/phoneix.conf```

Add two rows in this file. There is already a password that you will need later for setting up the UI so copy it to somewhere for now.
```
http-bind-ip=192.168.1.XX <-- This is the IP of the machine you install Phoneix Server on
http-bind-port=2121 <-- This is a port of your choice to use, remember what port you took
http-password=HeReIsYoUrPaSsWoRdThAtSAlReAdyInThIsFiLe <-- Change it if you want
```

Start Phoneix Server again
```./phoneixd```

###############################

Now its time for the UI for Phoneix Server
Start by create a file in /home/YourUserName

```nano appsettings.json```

Then add the following in that file
( on BaseUrl, dont use localhost or 127.0.0.1, use the machine lan ip as in the phoneix.conf file)
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "PhoenixApi": {
        "BaseUrl": "phoenix server ip and port",
        "Password": "phoenix server password"
    },
    "ApplicationSettings": {
        "Port": 5799
    }
}
```

press CTRL + x to close and press Y on the prompt to save

Now lets pull the UI ( check the repo for correct architecture, below is for amd64 )
[Avalible packages](https://github.com/Hodladi/HodladiWallet/pkgs/container/hodladiwallet%2Fhodlwallet)

```
docker pull ghcr.io/hodladi/hodladiwallet/hodlwallet:amd64-latest
```

Now run it!
```
docker run -v /home/YourUserName/host/appsettings.json:/app/appsettings.json -p 5799:5799 ghcr.io/hodladi/hodladiwallet/hodlwallet:amd64-latest
```
Remember to modify the path to YOUR appsettings.json in the above line.

Now the application is up and running. Its reachable at port 5799 from a browser.

Remember, if you want to be able to use the camera to scan QR codes, you need to setup self-signed certificates and use them with nginx, caddy or similar.
