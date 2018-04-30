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
        updateTracks();
    });


	skybox();
}

function updateTracks() {

    var totalTracks = trackMeshs.length;
    for (var i = 0; i < totalTracks; i++) {
        scene.remove(trackMeshs[i]);
    }
    trackMeshs = new Array();

    CreatesTracks([
        { x: 0, y: 0, z: 0, yaw: 0, pitch: 0 },
        { x: 7.7, y: 0, z: 0, yaw: 0, pitch: 0 },
        { x: 15.4, y: 0, z: 0, yaw: 0, pitch: 0 },
        { x: 23.1, y: 0, z: 0, yaw: 0, pitch: 0 }
    ], false);
  //  CreatesTracks([], true);

}

function CreatesTracks(tracks, color) {
    for (var i = 0; i < tracks.length; i++) {
        trackMeshs.push(CreateTrack(tracks[i].x, tracks[i].y, tracks[i].z, tracks[i].yaw, tracks[i].pitch, color));
    }
}
function CreateTrack(x, y, z, yaw, pitch, color) {
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
    trackMesh.scale.x = .135;
    trackMesh.scale.y = .135;
    trackMesh.scale.z = .135;

    //X, Y, Z
    trackMesh.position.x = -x * .025;
    trackMesh.position.y = z * .025;
    trackMesh.position.z = y * .025;

    //Rotate
    trackMesh.rotation.x = THREE.Math.degToRad(pitch);  //Pitch
    trackMesh.rotation.y = THREE.Math.degToRad(yaw + 90);
    trackMesh.rotation.z = 0; //Roll

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
