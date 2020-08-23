Hello folks,
after a long time the release 2.8.0 is ready. Here are the highlights

### New Features :rocket:
- You can undo/redo now (mostly). Yes, you read that right. Currently you can undo/redo the following
    - Adding of shapes and connections
    - Deleting of shapes and connections
    - Renaming shapes
    - Adding/inserting/renaming/modifying members of class shapes
    - Adding/inserting/renaming/modifying members of enums
    - Adding/inserting/renaming/modifying members of delegates
    - Pasting elements

    Moreover I've implemented an undo/redo visualizer ala Photoshop:
    So this is what I considered the MVP of the feauture. There maybe bugs and not everything is in place, more will follow

- The support for vs2015 is now dropped and support for vs2019 is now in place in the code generation
- NClass now uses an error details dialog when opening a file or importing a diagram. This should help find and troubleshoot problems faster
- NClass now checks for updates on application start.

### Bug fixes :bug:
- Fixed a weird behavior where shapes where dragged when the user did right click in them, caused by spurious win32 messages
- Fixed a small error when importing assemblies and there were internal only property
- Misc small stuff

### Next steps 📡 
So the work is going to continue with glacial pace, I'm doing this in my spare time, after all. The next release will focus on some java and code import improvements, most likely.

Thank you for your support and patience!