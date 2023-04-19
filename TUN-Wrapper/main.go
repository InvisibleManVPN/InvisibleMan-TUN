package main

import (
	"fmt"

	_ "github.com/invisiblemanvpn/tun-wrapper/tun"
)

func main() {
	fmt.Println("invisible man tun wrapper")
	fmt.Println("created by: invisiblemanvpn")
	fmt.Println("https://github.com/invisiblemanvpn")

	// for testing, you can use tun.RunTest(device, proxy) function.
	// uncomment these lines and replace the "device" variable with a name,
	// and replace the "proxy" variable with the proxy that you want to tunnel it.
	// NOTE: you need to run this as administrator

	// device := "for example: my-device"
	// proxy := "for example: 127.0.0.1:7890"
	// tun.RunTest(device, proxy)
}
