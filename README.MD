# AltV Resource Reload

Are you tired of having to copy your C# DLLs to the resource directory ? No more!

The problem with AltV is that dll files are locked when the resource is loaded and you cannot build straight to the resource directory.

This resource will monitor your build directory for changes, once any file is changed then the resource will be unloaded (to remove file lock) and copy the files to the resource directory once files are copied, your resource will be started again.

---

### Setup
1. Download resource files, copy to your server resources directory and add resource to your `server.toml` config.
2. Create a file `reloadconfig.json` in resources/ResourceReload 
3. Configure your resources as described below

---
### Example Config

```json
{
  "useAutoReconnect": true,
  "resources": [
    {
      "name": "ResourceName",
      "projects": [
        {
          "buildDirectory": "src/ResourceName/bin/Debug/net8.0",
          "targetDirectory": "resources/ResourceName/server/"
        },
        {
          "buildDirectory": "src/ResourceName/bin/Debug/net6.0",
          "targetDirectory": "resources/ResourceName/client/"
        }
      ]
    }
  ]
}
```
#### Explanation
In this example we have one resource with server-side and client-side files, in this case it should be configured as
a single resource. You can include as many projects as you want.

* `buildDirectory` is where your build artifacts live
* `targetDirectory` is the directory where files from `buildDirectory` will be copied to

When `useAutoReconnect` is set to true, resource reload will try to reconnect the local client **works only for local development**
as described in [altv docs](https://docs.altv.mp/articles/auto_reconnect.html#)

#### ⚠️ IMPORTANT
1. Paths are relative to `altv-server.exe`
2. Name property **must** match the name of your resource
3. Make sure all directories exist before starting the server