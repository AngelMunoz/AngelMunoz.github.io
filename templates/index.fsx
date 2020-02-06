#load "../siteModel.fsx"
#load "default.fsx"


open Html
open SiteModel

[<CLIMutable>]
type Model =
    { title: string }

let generate (siteModel: SiteModel) (mdl: Model) (posts: Post list) (content: string) =
    let posts =
        [ for post in posts do
            yield article [ Class "box box-index" ]
                      [ h1 [ Class "post-title" ] [ a [ Href post.link ] [ !!post.title ] ]
                        div [ Class "post-date" ]
                            [ (!!(defaultArg (post.published |> Option.map (fun p -> p.ToShortDateString())) "")) ] ] ]

    Default.defaultPage siteModel mdl.title posts
