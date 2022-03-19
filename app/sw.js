(function () {
  const VERSION = '{%VERSION%}';
  const files = [
    'images/favicon.svg',
    'images/header.jpg',
    'images/paw-heart.png',
    'images/paw-heart.svg',

    'app.js',
    'ce-carousel.js',
    'ce-details.js',
    'ce-item.js',
    'custom-elements.min.js',
    'helper.js',
    './',
    'manifest.json',
    'styles.css'
  ];

  self.oninstall = event => event.waitUntil(
    caches.open(`tinder4cats-${VERSION}`)
      .then(cache => cache.addAll(files))
  );

  self.onactivate = event => event.waitUntil(
    caches.keys()
      .then(cacheNames =>
        Promise.all(
          cacheNames
            .map(c => c.split('-'))
            .filter(c => c[0] === 'tinder4cats')
            .filter(c => c[1] !== VERSION)
            .map(c => caches.delete(c.join('-')))
        )
      )
  );

  self.onfetch = event => event.respondWith(
    caches.match(event.request)
      .then(response => response || fetch(event.request))
  );
})();