var controls, camera, scene, renderer, loader;
var layout, track, trackMeshs, trackGeometry, trackMaterials;

init();
animate();

function init() {
	scene = new THREE.Scene();

	/* Camera */
	camera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight, 0.01, 500000 );
	camera.position.z = 2;
	camera.position.set(0, 3.5, 5);
    camera.lookAt(scene.position);

	/* Lights */
	initLights();
	
	/* Renderer */
	renderer = new THREE.WebGLRenderer( { antialias: true, alpha: true } );
	renderer.setSize( window.innerWidth, window.innerHeight );
	document.body.appendChild( renderer.domElement );
	
	/* Resize */
	THREEx.WindowResize(renderer, camera)

	/* Controls */
	controls = new THREE.OrbitControls(camera, renderer.domElement);
	controls.enableDamping = true;
	controls.dampingFactor = .15;
	controls.rotateSpeed = 0.25; 
	controls.enableZoom = true;
	controls.maxPolarAngle  = Math.PI / 2.1;
	controls.minPolarAngle  = 0;

	/* Events */
	//window.addEventListener('resize', onWindowResize, false);

	

	/* Load Models */
    trackMeshs = new Array();
    var loader = new THREE.JSONLoader();
    loader.load('./assets/layout.json', function (geometry, materials) {
        layout = new THREE.Mesh(geometry, materials);
        scene.add(layout);
    });
    loader.load('./assets/track.json', function (geometry, materials) {
        trackGeometry = geometry;
        trackMaterials = materials;
        track = new THREE.Mesh(geometry, materials);
        track.traverse(function (object) {
            if (object instanceof THREE.Mesh) {
                object.castShadow = true;
                object.receiveShadow = true;
            }
        });
    });
	skybox();
}
function CoasterUpdate(update) {
    for (var i = 0; i < update.RemovedTracksCount; i++) {
        scene.remove(trackMeshs[trackMeshs.length - 1]);
        trackMeshs.pop();
    }
    CreatesTracks(update.NewTracks, false);
}
function CreatesTracks(tracks, color) {
    for (var i = 0; i < tracks.length; i++) {
        //[ TO DO ] Determine Color By location in Array
        trackMeshs.push(CreateTrack(tracks[i], false));
    }
}
function CreateTrack(track, color) {
    var trackMesh;

    //Material
    if (color == true) {
        material = new THREE.MeshBasicMaterial({ color: color, wireframe: false });
    }
    else {
        material = new THREE.MeshFaceMaterial(trackMaterials);
    }

    //Create Mesh
    trackMesh = new THREE.Mesh(trackGeometry, material);

    //Scale
    trackMesh.scale.x = .205;
    trackMesh.scale.y = .205;
    trackMesh.scale.z = .205;

    //X, Y, Z
    trackMesh.position.x = -track.X * .036 + 18.5;
    trackMesh.position.y = track.Z * .036;
    trackMesh.position.z = track.Y * .036 + 8.55;

    //Rotate
    trackMesh.eulerOrder = 'ZYX';
    trackMesh.rotation.x = THREE.Math.degToRad(track.Pitch);  //Pitch
    trackMesh.rotation.y = THREE.Math.degToRad(track.Yaw + 90);
    trackMesh.rotation.z = 0;

    //Shadow
    trackMesh.traverse(function (object) {
        if (object instanceof THREE.Mesh) {
            object.castShadow = true;
            object.receiveShadow = true;
        }

    });

    trackMesh.receiveShadow = true;
    scene.add(trackMesh);

    return trackMesh;
}
function animate() {
	requestAnimationFrame( animate );
    controls.update();
	renderer.render( scene, camera );

}
function initLights() {
   // var light = new THREE.AmbientLight(0xffffff, 0.3);
   // scene.add(light);

   var light;

   light = new THREE.DirectionalLight(0xdfebff, 2.2);
    light.position.set(0, 400, 50);
    light.position.multiplyScalar(1);

    light.castShadow = true;

    light.shadowMapWidth = 5024;
   light.shadowMapHeight = 5024;

    var d = 200;
    light.position.y = 4800;
    light.shadowCameraLeft = -1000;
    light.shadowCameraRight = 1000;
    light.shadowCameraTop = 1000;
    light.shadowCameraBottom = -1000;

    light.shadowCameraFar = 5000;
    light.shadowDarkness = 0.2;

    scene.add(light);

}
function initMesh() {
    loader.load('./assets/layout.json', function(geometry, materials) {
        layout = new THREE.Mesh(geometry, materials);
        scene.add(layout);
    });
}
function initTrack() {
    loader.load('./assets/track.json', function (geometry, materials) {
        mesh = new THREE.Mesh(geometry, materials);
        scene.add(mesh);
    });

}
function skybox(){
	var geometry = new THREE.CubeGeometry(10000,10000,10000);
	var cubeMaterials = [
		new THREE.MeshBasicMaterial( {map:new THREE.TextureLoader().load("./assets/textures/front.png"), side: THREE.DoubleSide}),
		new THREE.MeshBasicMaterial( {map:new THREE.TextureLoader().load("./assets/textures/back.png"), side: THREE.DoubleSide}),
		new THREE.MeshBasicMaterial( {map:new THREE.TextureLoader().load("./assets/textures/up.png"), side: THREE.DoubleSide}),
		new THREE.MeshBasicMaterial( {map:new THREE.TextureLoader().load("./assets/textures/down.png"), side: THREE.DoubleSide}),
		new THREE.MeshBasicMaterial( {map:new THREE.TextureLoader().load("./assets/textures/right.png"), side: THREE.DoubleSide}),
		new THREE.MeshBasicMaterial( {map:new THREE.TextureLoader().load("./assets/textures/left.png"), side: THREE.DoubleSide}),
	];
var cubeMaterial = new THREE.MeshFaceMaterial( cubeMaterials );
var cube = new THREE.Mesh(geometry, cubeMaterial);
scene.add( cube);
}
