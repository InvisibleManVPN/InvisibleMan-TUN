# Invisible Man - TUN

> Tunneling service for Invisible Man applications (uses tun2socks and wintun)

Invisible Man TUN is an open-source and free service that allows you to tunnel all of your system's traffic. It uses [tun2socks](https://github.com/xjasonlyu/tun2socks) and [wintun](https://www.wintun.net).
<br/>
This service creates the network interface and sets routes automatically.

## Getting started

- If you just want to use this service: 
  - Download it from [releases](https://github.com/InvisibleManVPN/InvisibleMan-TUN/releases/latest).
  - Start the service by this instruction:
    ```
    InvisibleMan-TUN -port={port}
    ```

- But if you want to get the source of the service, follow these steps:
  - Download the [requirements](#requirements)
  - Clone a copy of the repository:
    ```
    git clone "https://github.com/InvisibleManVPN/InvisibleMan-TUN.git"
    ```
  - Change to the directory:
    ```
    cd InvisibleMan-TUN
    ```
  - Download one of the versions of [tun2socks](https://github.com/xjasonlyu/tun2socks/releases/latest) based on your OS and extract it to `/InvisibleMan-TUN` directory.
    <br/>
    **NOTE:** After extracting the file, change its name to `tun2socks.exe`.
  - Download [wintun](https://www.wintun.net), extract it and copy one of versions based on your OS to `/InvisibleMan-TUN` directory.
    <br/>
    **NOTE:** Make sure the dll file name is `wintun.dll`.
  - Run the project as administrator:
    ```
    dotnet run -port={port}
    ```

## Communicate with the service

After running the service on a specific port, you need to connect to the service via a `socket`. So, you should open a `socket` to the chosen `port`. This is the port that allows your application and service to communicate with each other. Then you can use commands to control the service. Currently, we have two valid commands:

- `enable`: Enables the tunneling service and set a network interface.
    ```
    -command=enable -device={device} -proxy={ip}:{port} -address={address} -server={server} -dns={dns}
    ```
- `disable`: Disables the tunneling service and remove the network interface.
    ```
    -command=disable
    ```

## Requirements

- .Net https://dotnet.microsoft.com/download

## Contacts

- Web [invisiblemanvpn.github.io](https://invisiblemanvpn.github.io)
- Email [invisiblemanvpn@gmail.com](mailto:invisiblemanvpn@gmail.com)