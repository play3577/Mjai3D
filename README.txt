3D GUI client for mjai.
Version: 0.0.2

You can connect to a mjai server and play majong.

Currently, core features are almost implemented and other features aren't.

For example, we can change DEBUG only by editing "Program.cs".(when DEBUG is true, input is ../input3.txt. when DEBUG is false, input is TCP Socket. see "Program.cs".)

That is the same with the Socket destination.(you may think this is fatal...)

To build this project, you also need Wistery.Majong project.

TODO:
State Transition(Menu, Options, ...)
Animation(You can't tell whether his dahai is tsumogiri or not.)
Hora detail
Show dora marker
...

Version 0.0.2: Moved core majong classes from Mjai3D to Wistery.Majong
Version 0.0.1: first version