package tun

import (
	"C"
	"fmt"
	"os/exec"
	"syscall"

	"golang.org/x/sys/windows"
)

var cmd *exec.Cmd

//export StartTun
func StartTun(device *C.char, proxy *C.char) {
	args := []string{
		"-device", C.GoString(device),
		"-proxy", C.GoString(proxy),
	}

	cmd = exec.Command("./tun2socks", args...)
	err := cmd.Run()

	if err != nil {
		fmt.Println("error | failed to start tun2socks >", err)
		return
	}
}

//export StopTun
func StopTun() {
	dll, err := windows.LoadDLL("kernel32.dll")
	if err != nil {
		fmt.Println("error | failed to load dll >", err)
	}

	defer dll.Release()

	pid := cmd.Process.Pid
	proc, err := dll.FindProc("AttachConsole")
	if err != nil {
		fmt.Println("error | failed to find proc >", err)
	}

	r1, _, err := proc.Call(uintptr(pid))
	if r1 == 0 && err != syscall.ERROR_ACCESS_DENIED {
		fmt.Println("error | failed to call proc >", err)
	}

	proc, err = dll.FindProc("SetConsoleCtrlHandler")
	if err != nil {
		fmt.Println("error | failed to find proc >", err)
	}

	r1, _, err = proc.Call(0, 1)
	if r1 == 0 {
		fmt.Println("error | failed to call proc >", err)
	}

	proc, err = dll.FindProc("GenerateConsoleCtrlEvent")
	if err != nil {
		fmt.Println("error | failed to find proc >", err)
	}

	r1, _, err = proc.Call(windows.CTRL_BREAK_EVENT, uintptr(pid))
	if r1 == 0 {
		fmt.Println("error | failed to call proc >", err)
	}
}
