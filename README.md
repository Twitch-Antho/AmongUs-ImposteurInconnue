# Among Us Imposteur Inconnue
ATTENTION ⚠️ | ANNONCE !

![images](https://github.com/user-attachments/assets/0f10a1f2-4b4b-4ba3-aa56-2219a4041d5f)

## Caractères

-  Les imposteurs ne se connaissent pas (intro, noms, réunions et chat)
-  Les imposteurs peuvent s'entre-tuer
-  Les imposteurs sont touchés par le sabotage des lumières, avec une vision réglable
-  Le mode est réglé par l'hôte depuis le chat du lobby et se désactive automatiquement si l'hôte n'a pas le mod

## Version

Compatible avec **Among Us 2026.9.29** (Steam), **BepInEx 6.0.0-be.735 (IL2CPP)** et **Reactor 2.5.1**.

## Commandes du lobby (hôte)

| Commande | Effet |
|---|---|
| `/inconnu` | Affiche les réglages actuels |
| `/inconnu on` / `/inconnu off` | Active ou désactive le mode |
| `/inconnu vision 1.5` | Vision des imposteurs quand les lumières sont coupées (0.25 à 5) |

Les réglages sont aussi enregistrés dans `BepInEx/config/com.herysia.amongusunkimpostor.cfg`.

## Technique

Ce mod a été réalisé à l'aide du [framework de modding Reactor](https://github.com/NuclearPowered/Reactor), basé sur BepInEx, il corrige le jeu au moment de l'exécution et **NE** modifie aucun fichier de jeu.

### Installation

Tous les joueurs doivent avoir installé le mod pour la meilleure expérience utilisateur

-   Téléchargez la [dernière version](https://github.com/Twitch-Antho/AmongUs-ImposteurInconnue/releases/latest) : `ImposteurInconnu-Steam-Itch.zip` pour Steam/Itch, `ImposteurInconnu-Epic-MicrosoftStore.zip` pour Epic/Microsoft Store.
-   Extraire les fichiers dans le dossier du jeu Among Us (`steam/steamapps/common/Among us`)
-   Cela devrait ressembler à ceci
    ![looklikethis](./Visuals/looklikethis.png)
-   **Exécutez le jeu depuis Steam**

### Remarque relative à l'installation

Si vous souhaitez installer Reactor par vous-même, installez [BepInEx 6 IL2CPP be.735](https://builds.bepinex.dev/projects/bepinex_be) (x86 pour Steam/Itch, x64 pour Epic/Microsoft Store), puis [Reactor](https://github.com/NuclearPowered/Reactor/releases) dans `BepInEx/plugins`. Et puis copiez la dll du plugin (`AmongUsUnknownImpostors.dll` dans les [releases](https://github.com/Twitch-Antho/AmongUs-ImposteurInconnue/releases/latest)) dans `Among us/BepInEx/plugins`
Si vous souhaitez jouer sur des serveurs officiels, vous devez alors désactiver l'option de handshake personnalisé à partir de : `Among us/BepInEx/config/gg.reactor.api.cfg`

### Désinstaller

Si vous souhaitez désinstaller uniquement ce mod, supprimez la dll `Among us/BepInEx/plugins/AmongUsUnknownImpostors.dll`.

Si vous souhaitez le désactiver, vous pouvez renommer ou supprimer temporairement le fichier `Among us/winhttp.dll` ou désinstaller le jeu pour le réinstaller dans le mod.

Si vous souhaitez désinstaller complètement Reactor/BepInEx, supprimez les fichiers et dossiers suivants

```
+-- BepInEx
+-- dotnet
+-- .doorstop_version
+-- changelog.txt
+-- doorstop_config.ini
+-- winhttp.dll
```

# Contribution

Vous avez rencontré un bug ou un comportement inattendu ? Vous souhaitez proposer ou ajouter une nouvelle fonctionnalité ?
Pour obtenir une aide générale sur les problèmes, les suggestions de fonctionnalités ou les bugs, rejoignez le <a href="https://discord.com/invite/ad7aMevNMx" target="_blank">Discord</a>
Ou réaliser un [Ticket](https://github.com/AnthoYt/Royale-Us/issues/17)

# Licences et crédits

AmongUsUnknownImpostors est sous licence MIT. Voir [LICENSE](LICENSE.md) pour la licence complète.

Crédits à [@Galster](https://github.com/Galster-dev) pour les paramètres de lobby personnalisés et à [@Herysia](https://github.com/Herysia) pour avoir mis en oevre le premier projet.
Et [@Antho](https://github.com/Twitch-Antho) pour l'avoir remit à jour pour une nouvelle expérience sur Among Us.

# Contact
Discord :
antho_off [Serveur](https://discord.com/invite/ad7aMevNMx)

Youtube :
<a href="https://www.youtube.com/@Royale_Antho" target="_blank">Antho_Off</a>
