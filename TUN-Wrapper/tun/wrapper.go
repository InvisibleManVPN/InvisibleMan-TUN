package tun

import (
	"C"
	"fmt"
	"os/exec"
	"syscall"

	"golang.org/x/sys/windows"
)

var cmd *exec.Cmd

//export StartTunnel
func StartTunnel(device *C.char, proxy *C.char) {
	args := []string{
		"-device", C.GoString(device),
		"-proxy", C.GoString(proxy),
	}

	cmd = exec.Command("./tun2socks", args...)
	cmd.SysProcAttr = &syscall.SysProcAttr{
		CreationFlags:    syscall.CREATE_NEW_PROCESS_GROUP,
		NoInheritHandles: false,
		HideWindow:       false,
	}

	err := cmd.Start()

	if err != nil {
		fmt.Println("error | failed to start tun2socks >", err)
		return
	}

	fmt.Println("starting tun2socks - pid:", cmd.Process.Pid)
}

//export StopTunnel
func StopTunnel() {
	dll, err := windows.LoadDLL("kernel32.dll")
	if err != nil {
		fmt.Println("error | failed to load dll >", err)
	}

	defer dll.Release()

	pid := cmd.Process.Pid
	proc, err := dll.FindProc("AttachConsole")
	if err != nil {
		fmt.Println("error | failed to find proc >", "AttachConsole", err)
	}

	r1, _, err := proc.Call(uintptr(pid))
	if r1 == 0 && err != syscall.ERROR_ACCESS_DENIED {
		fmt.Println("error | failed to call proc >", "uintptr(pid) - syscall.ERROR_ACCESS_DENIED", err)
	}

	proc, err = dll.FindProc("SetConsoleCtrlHandler")
	if err != nil {
		fmt.Println("error | failed to find proc >", "SetConsoleCtrlHandler", err)
	}

	r1, _, err = proc.Call(0, 1)
	if r1 == 0 {
		fmt.Println("error | failed to call proc >", "proc.Call(0, 1)", err)
	}

	proc, err = dll.FindProc("GenerateConsoleCtrlEvent")
	if err != nil {
		fmt.Println("error | failed to find proc >", "GenerateConsoleCtrlEvent", err)
	}

	r1, _, err = proc.Call(windows.CTRL_BREAK_EVENT, uintptr(pid))
	if r1 == 0 {
		fmt.Println("error | failed to call proc >", "uintptr(pid) - windows.CTRL_C_EVENT", err)
	}

	fmt.Println("stopping tun2socks - pid:", pid)
}
