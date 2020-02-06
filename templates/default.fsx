#load "../siteModel.fsx"

open Html
open SiteModel

let defaultPage (siteModel: SiteModel) pageTitle content =
    html []
        [ head []
              [ meta [ CharSet "utf-8" ]
                title [] [ (!!pageTitle) ]
                link
                    [ Rel "stylesheet"
                      Type "text/css"
                      Href "/css/bulma.min.css" ]
                link
                    [ Rel "stylesheet"
                      Type "text/css"
                      Href "/css/style.css" ]
                link
                    [ Rel "alternate"
                      Type "application/atom+xml"
                      Href "/feed.xml"
                      HtmlProperties.Title "News Feed" ] ]
          body []
              [ main [ Class "hero is-fullheight is-dark is-bold" ]
                    [ div [ Class "hero-head" ]
                          [ header [ Class "navbar" ]
                                [ div [ Class "container" ]
                                      [ div [ Class "navbar-menu" ]
                                            [ div [ Class "navbar-end" ]
                                                  [ a
                                                      [ Class "navbar-item"
                                                        Href "/" ] [ !!(siteModel.title) ]
                                                    a
                                                        [ Class "navbar-item"
                                                          Href "/archive.html" ] [ !!"Archive" ] ] ] ] ] ]
                      div [ Class "hero-body container is-column" ]
                          [ yield h1 [ Class "title" ] [ !!siteModel.title ]
                            for cont in content do
                                yield cont ]
                      div [ Class "hero-foot" ]
                          [ nav [ Class "tabs is-toggle is-toggle-rounded is-fullwidth is-large" ]
                                [ div [ Class "container" ]
                                      [ ul []
                                            [ li [] [ a [ Target "_blank" ; Href "https://github.com/AngelMunoz/"] [ !!"Github" ] ]
                                              li [] [ a [ Target "_blank" ; Href "https://gitlab.com/AngelMunoz/"] [ !!"Gitlab" ] ]
                                              li [] [ a [ Target "_blank" ; Href "https://twitter.com/daniel_tuna/"] [ !!"Twitter" ] ]
                                              li [] [ a [ Target "_blank" ; Href "https://www.linkedin.com/in/angeldmunoz/"] [ !!"Linked In" ] ] ] ] ] ] ] ] ]
