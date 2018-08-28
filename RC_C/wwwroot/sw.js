const version = "0.6.11";
const cacheName = `airhorner-${version}`;
self.addEventListener('install', e => {
  e.waitUntil(
    caches.open(cacheName).then(cache => {
      return cache.addAll([
        `/`,
        `/index.html`,
        `/assets/textures/IconMap.png`,
        `/css/site.css`,
        `/js/rc.js`,
        `/js/compressed.js`,
        `/js/main.js`,
        `/js/RC.wasm`,
        `/assets/Layout.json`,
        `/assets/textures/front.png`,
        `/assets/textures/back.png`,
        `/assets/textures/up.png`,
        `/assets/textures/down.png`,
        `/assets/textures/left.png`,
        `/assets/textures/right.png`,
        `/assets/WoodTrack.json`,
        `/assets/SupportTrack.json`,
        `/assets/textures/wood.png`
      ])

    })
  );
});

self.addEventListener('activate', event => {
  event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', event => {
  event.respondWith(
    caches.open(cacheName)
      .then(cache => cache.match(event.request, {ignoreSearch: true}))
      .then(response => {
      return response || fetch(event.request);
    })
  );
});
