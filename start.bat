#!/bin/bash

start .\main-linux
get_char()
{
    SAVEDSTTY= stty -g
    stty -echo
    stty raw
    dd if=/dev/tty bs=1 count=1 2> /dev/null
    stty -raw
    stty echo
    stty $SAVEDSTTY
}
if [ -z "$1" ]; then
    echo'请按任意键非续...
else
    echo -e "$1"
fi
get char