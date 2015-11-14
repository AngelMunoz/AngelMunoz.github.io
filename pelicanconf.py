#!/usr/bin/env python
# -*- coding: utf-8 -*- #
from __future__ import unicode_literals

AUTHOR = u'Angel Munoz'
SITENAME = u"AngelMunoz's Blog"
SITESUBTITLE = u"Tunaxor's blog"
SITEURL = 'http://angelmunoz.github.io'

DISQUS_SITENAME = "tunaxorsblog"
GITHUB_URL = "http://github.com/angelmunoz/angelmunoz.github.io"
TWITTER_USERNAME = "Daniel_Tuna"


# Languaje and locale options
DEFAULT_LANG = u'en'
DATE_FORMATS = {
    'en':'%a, %m/%d/%Y',
    'es':'%a, %d/%m/%Y',
}
TIMEZONE = 'America/Hermosillo'
LOCALE = {
    'us', 'mex',
    'en_US', 'es_MX'
}

# Feed generation is usually not desired when developing
FEED_ALL_ATOM = None
CATEGORY_FEED_ATOM = None
TRANSLATION_FEED_ATOM = None
AUTHOR_FEED_ATOM = None
AUTHOR_FEED_RSS = None

# Blogroll
LINKS = (('Pelican', 'http://getpelican.com/'),
         ('Python.org', 'http://python.org/'),
         ('Jinja2', 'http://jinja.pocoo.org/'),
         ('You can modify those links in your config file', '#'),)

# Social widget
SOCIAL = (('Twitter', 'http://twitter.com/Daniel_Tuna'),
          ('Facebook', 'https://www.facebook.com/danieltunamunoz'),
          ('G+', u'https://plus.google.com/+AngelDanielMuñozGonzalez'),
          ('Github', 'https://github.com/AngelMunoz'),
          ('linkedin', 'https://mx.linkedin.com/in/danieltuna'))



DEFAULT_PAGINATION = 5

# Uncomment following line if you want document-relative URLs when developing
RELATIVE_URLS = True

PATH = 'content'
ARTICLE_PATHS = ['blog']
ARTICLE_SAVE_AS = '{date:%Y}/{slug}.html'
ARTICLE_URL = '{date:%Y}/{slug}.html'

# Theme options
THEME = 'theme'
