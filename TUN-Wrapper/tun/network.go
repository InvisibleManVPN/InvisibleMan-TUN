package tun

import (
	"C"
	"fmt"
	"os/exec"
	"strconv"
)

//export SetInterfaceAddress
func SetInterfaceAddress(device *C.char, address *C.char) {
	process := "netsh"
	args := []string{
		"interface", "ip", "set", "address",
		"name=" + C.GoString(device),
		"addr=" + C.GoString(address),
		"source=static",
		"mask=255.255.255.0",
		"gateway=none",
	}

	runProcess(process, args)
}

//export SetInterfaceDns
func SetInterfaceDns(device *C.char, dns *C.char) {
	process := "netsh"
	args := []string{
		"interface", "ip", "set", "dns",
		"name=" + C.GoString(device),
		"static",
		C.GoString(dns),
	}

	runProcess(process, args)
}

//export SetRoutes
func SetRoutes(server *C.char, address *C.char, gateway *C.char, index int) {
	process := "cmd"
	args := []string{
		"/c", "route", "add",
		C.GoString(server),
		"mask", "255.255.255.255",
		C.GoString(gateway),
	}

	runProcess(process, args)

	process = "cmd"
	args = []string{
		"/c", "route", "add",
		"0.0.0.0", "mask", "0.0.0.0",
		C.GoString(address),
		"IF", strconv.Itoa(index),
	}

	runProcess(process, args)
}

func runProcess(process string, args []string) {
	cmd := exec.Command(process, args...)
	err := cmd.Run()

	if err != nil {
		fmt.Println("error | failed to start process >", process, err)
		return
	}
}
