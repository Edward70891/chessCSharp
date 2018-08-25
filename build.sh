#!/bin/bash

mcs -t:library chess.cs
mcs -r:chess.dll main.cs
