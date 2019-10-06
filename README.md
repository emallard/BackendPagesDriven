# Concept

Le back définit la structure des pages du site et les enchainements entre celles-ci 
que ce soit par des liens ou par des formulaires propsant des redirections.
Le front n'aura aucune url à manipuler, elles seront générées à la volée par le back.

## Explications

Les pages sont déclarées en 3 fois :
- PageQuery : c'est elle qui sera mappée à une url dans le navigateur
- Page : contient les données de la page à afficher
- PageModule : permet d'écrire le Handler (et plus)

Une page peut être composée :
- de liens : c'est à dire des objets de type PageQuery
- de données issues de requetes métier : AsyncCall
- de formulaires contentant des commandes métier et proposant une redirection vers la page suivante : Form

Un AsyncCall représente une query métier et une réponse pour la page. Cette réponse sera généralement une transformation
de la réponse maétier pour y inclure des liens.

Un Form représente une Command et la redirection vers la page suivante. Si la commande crée une ressource, le lien pourra 
contenir le Guid de cette ressource.

## Les Tests

Les tests s'écrivent comme si on manipulait un navigateur. On peut : 
- Afficher une page
- Suivre un lien
- Remplir des champs de formulaires
- Soumettre le formulaire et passer à la page suivante

Deux implementations de l'interfaace IBrowser existent : 
- une utilisées poru les tests unitaires
- une qui utilise le WebDriver Selenium

## Exploitation des tests via l'application PageLogs

L'éxécution des tests va être espionnée pour produire des données:
- Enchainements des pages
- Requetes et Commandes éxécutées
- Entités lues et écrites
- Mail envoyés

Ces données sont représentées dans l'application accessible par défaut sur http://localhost:5000/pagelogs

## Interface générique

Un moteur de rendu de pages générique est fourni et propose un affichage par défaut des objets de type Page.
Il est facilement personnalisable pour amélorier ponctuellement la lisibilité du site.
Il n'est pas destiné à la version finale du produit mais à créer une maquette qui validera la spécification client.

# Exemples

## Faire des liens entre les pages

```csharp
class ArticlePageQuery
{
    public string NomArticle;
}

class AccueilPageQuery {

}

class AccueilPage {
    public ArticlePageQuery LienArticle;
}

class AccueilPageModule : PageModule
{
    public AccueilPageModule()
    {
        HandlePage<AcceuilPageQuery, AccueilPage>((query, page) => {
            page.LienArticle = new ArticlePAgeQuery() { NomArticle="a-la-une" };
        });
    }
}
```


# Todos

- Pour une page tester tous les inputs invalides.
  Peut-être avec une syntaxe Command.Champ.IsRequired() => regarde s'il y a erreur de validation.
