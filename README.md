# Simple Beatmap Play Count (Asset Bundler Unity project)

> **Warning**:
> This is **not** the Simple Beatmap Play Count mod. This is a Unity project to package assets for the mod in a format read by the mod.

### Usage

1. Open the project in Unity 2019.4.28f1
1. Go to Tools → Object Exporter to open the Object Exporter window
1. Put any assets on the `Assets/In` folder
1. Hit Export
1. Bundle.bundle files will be generated for `StandaloneWindows` and `Android` under the `Assets/Out` folder

### FAQ

#### Isn't it overkill to have a whole .bundle file for just this one image? Why not just embed the image and load the image directly?

Yes.

---

Jokes aside, I noticed way too late that there was a better way to do this, so I just continued with how I was doing it.

I also needed an excuse for a repository where to put the original SVG file.
Otherwise where would I put it? The PC version repo? The Quest version repo?

### Credits

Special thanks to [RedBrumbler](https://github.com/RedBrumbler) for the original ObjectExporter.cs file which I graciously took from an old Discord conversation.