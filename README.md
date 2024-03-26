This is a .NET 8 Blazor Web application that works as an UI for ([Phoenix Server](https://phoenix.acinq.co/server)

It's avalible for ARM64, ARMv7 and AMD64 architecture, see packages to determine which to pull.

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

docker pull ghcr.io/hodladi/hodladiwallet/hodlwallet:main-caxxxx-axxxx

docker run -v /path/on/host/appsettings.json:/app/appsettings.json -p 5799:5799
```
This will start the container and the application will listen to port 5799 in a browser.
