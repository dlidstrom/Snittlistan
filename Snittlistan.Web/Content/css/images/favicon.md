# Favicon script

```bash
npm install -g cli-real-favicon
```

## faviconDescription.json

```json
{
    "masterPicture": "TODO: Path to your master picture",
    "iconsPath": "/",
    "design": {
        "ios": {
            "pictureAspect": "backgroundAndMargin",
            "backgroundColor": "#333333",
            "margin": "0%",
            "assets": {
                "ios6AndPriorIcons": false,
                "ios7AndLaterIcons": false,
                "precomposedIcons": false,
                "declareOnlyDefaultIcon": true
            }
        },
        "desktopBrowser": {
            "design": "background",
            "backgroundColor": "#333333",
            "backgroundRadius": 0.45,
            "imageScale": 0.8
        },
        "windows": {
            "pictureAspect": "noChange",
            "backgroundColor": "#da532c",
            "onConflict": "override",
            "assets": {
                "windows80Ie10Tile": false,
                "windows10Ie11EdgeTiles": {
                    "small": false,
                    "medium": true,
                    "big": false,
                    "rectangle": false
                }
            }
        },
        "androidChrome": {
            "pictureAspect": "noChange",
            "themeColor": "#ffffff",
            "manifest": {
                "display": "standalone",
                "orientation": "notSet",
                "onConflict": "override",
                "declared": true
            },
            "assets": {
                "legacyIcon": false,
                "lowResolutionIcons": false
            }
        },
        "safariPinnedTab": {
            "pictureAspect": "silhouette",
            "themeColor": "#5bbad5"
        }
    },
    "settings": {
        "scalingAlgorithm": "Mitchell",
        "errorOnImageTooSmall": false,
        "readmeFile": false,
        "htmlCodeFile": false,
        "usePathAsIs": false
    }
}
```

```bash
real-favicon generate faviconDescription.json faviconData.json outputDir
```
