var controls, camera, scene, renderer, loader;
var layout, track, trackMeshs, trackGeometry, trackMaterials;
var stats;
init();
animate();

function init() {


    scene = new THREE.Scene();

    /* Camera */
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.01, 500000);
    camera.position.z = 2;
    camera.position.set(0, 3.5, 5);
    camera.lookAt(scene.position);

    /* Lights */
    initLights();

    //TEST

    /* Renderer */
    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setPixelRatio(window.devicePixelRatio);
    document.body.appendChild(renderer.domElement);

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

    stats = new Stats();
    stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
    document.body.appendChild(stats.dom);
}
function CoasterUpdate(added, removed) {
    for (var i = 0; i < removed; i++) {
        scene.remove(trackMeshs[trackMeshs.length - 1]);
        trackMeshs.pop();
    }
    CreatesTracks(added, false);
}
function CreatesTracks(added, color) {
    for (var i = 0; i < added; i++) {
        //[ TO DO ] Determine Color By location in Array

        console.log("X : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20));
        console.log("Y : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 4));
        console.log("Z : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 8));
        console.log("Yaw : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 12));
        console.log("Pitch : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 16));

        trackMeshs.push(CreateTrack((trackMeshs.length) * 20, false));
    }
}
function CreateTrack(trackIndex, color) {
    var trackMesh;

    //Material
    if (color == true) {
        material = [{ color: color, wireframe: false }];
    }
    else {
      //  material = [trackMaterials];
    }

    //Create Mesh
    trackMesh = new THREE.Mesh(trackGeometry, trackMaterials);

    //Scale
    trackMesh.scale.x = .205;
    trackMesh.scale.y = .205;
    trackMesh.scale.z = .205;

    //X, Y, Z
    trackMesh.position.x = -Blazor.platform.readFloatField(dataReference, trackIndex) * .036 + 18.5; //X
    trackMesh.position.y = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * .036; //Z
    trackMesh.position.z = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * .036 + 8.55; //Y
    //Data[i * 5] = NewTracks[i].X;
    //Data[i * 5 + 1] = NewTracks[i].Y;
    //Data[i * 5 + 2] = NewTracks[i].Z;
    //Data[i * 5 + 3] = NewTracks[i].Yaw;
    //Data[i * 5 + 4] = NewTracks[i].Pitch;

    //Rotate
    trackMesh.rotation.order = 'ZYX';
    trackMesh.rotation.x = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 16));  //Pitch
    trackMesh.rotation.y = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90);
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
    stats.begin();
    controls.update();
    renderer.render(scene, camera);
    stats.end();
    requestAnimationFrame(animate);
}
function initLights() {
   // var light = new THREE.AmbientLight(0xffffff, 0.3);
   // scene.add(light);

   var light;

   light = new THREE.DirectionalLight(0xdfebff, 2.2);
   light.position.set(0, 400, 50);
   light.position.multiplyScalar(1);

   light.castShadow = true;

   light.shadow.mapSize.width = 5024;
   light.shadow.mapSize.height = 5024;

    var d = 200;
    light.position.y = 4800;
    light.shadow.camera.left = -1000;
    light.shadow.camera.right = 1000;
    light.shadow.camera.top = 1000;
    light.shadow.camera.bottom = -1000;
    light.shadow.camera.far = 5000;

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
    var cube = new THREE.Mesh(geometry, cubeMaterials);
scene.add( cube);
}
