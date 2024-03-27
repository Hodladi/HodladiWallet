This is a .NET 8 Blazor Web application that works as an UI for [Phoenix Server](https://phoenix.acinq.co/server)

It's also a PWA application so you can install it from your web browser.

Things that work:
- See wallet balance
- Create invoice with specifiec amount ( gives you QR and invoice string )
- Scan QR with camera ( Requires app running on https )
- Paste invoice string to pay invoice
- See amount of invoice pasting / scanning before pay

Things that DONT work (yet):
- Pay invoice with no set amount
- Pay to lightningaddress
- Pay LNRUL invoices
- Create invoice with no amount
- There is no notifications on receive payment ( else than balance higher all of a sudden )
- There is no notification on send payment success ( else than balance lower after try send than before )

As you can see above it's only the basic functionalitys working right now and as mentioned above. If you want to use the camera on your device the application needs to run through HTTPS. You need to setup nginx, caddy or similar with self-signed certificates.

It's avalible for ARM64, ARMv7 and AMD64 architecture, see packages to determine which to pull.

[More detailed installation instructions](https://github.com/Hodladi/HodladiWallet/blob/main/INSTALLATION.md)

To use this application you need to add a appsettings.json file that should contain the below.
Place the appsettings.json file at a suitable place and relate to the file path in the next step.
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
        "BaseUrl": "http://ip-to-Phoneix-server:port",
        "Password": "password-to-phoneix-server"
    }
}
```
To run the application, use below command but replace /path/on/host/appsettings.json with the path to your appsettings.json
```
**Example to run. Change according to version and hash**

docker pull ghcr.io/hodladi/hodladiwallet/hodlwallet:amd64-latest

docker run -v /path/on/host/appsettings.json:/app/appsettings.json -p 5799:5799 ghcr.io/hodladi/hodladiwallet/hodlwallet:amd64-latest
```
This will start the container and the application will listen to port 5799 in a browser.

![image](https://github.com/Hodladi/HodladiWallet/assets/91490683/a76774aa-6a97-47cf-be2a-199471678487)

![image](https://github.com/Hodladi/HodladiWallet/assets/91490683/e5e1070b-c160-4418-a952-76381c5fa65f)

![image](https://github.com/Hodladi/HodladiWallet/assets/91490683/7804d8e0-b9d7-407b-8cff-b1dfec958356)

![image](https://github.com/Hodladi/HodladiWallet/assets/91490683/7cc23af4-0054-4d08-a118-d273e02c362b)

![image](https://github.com/Hodladi/HodladiWallet/assets/91490683/903ba150-549a-4145-a6f9-6894adcf5248)
