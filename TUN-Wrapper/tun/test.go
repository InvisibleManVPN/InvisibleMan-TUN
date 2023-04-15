package tun

import (
	"C"
	"fmt"
)

func RunTest(device string, proxy string) {
	fmt.Println("start testing...")
	cdevice := C.CString(device)
	cproxy := C.CString(proxy)

	fmt.Println("device:", device)
	fmt.Println("proxy:", proxy)

	StartTun(cdevice, cproxy)
}
