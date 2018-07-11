var controls, camera, scene, renderer, loader;
var layout, track, trackMeshs, trackGeometry, trackMaterials;
var stats;
var trackCount;
var objectCount, instanceCount = 100, tracksMatrix, mcol0, mcol1, mcol2, mcol3, tracksMatrixMax = 1000, matrix, me, bgeo;
var mcol0, mcol1, mcol2, mcol3;
var bufferGeometry, geometry, offsets = [], orientations = [];
var kOffSets, offsetAttribute, orientationAttribute;
init();
animate();

function init() {

    trackCount = 0;
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0xf0f0f0); // UPDATED
    matrix = new THREE.Matrix4();
    me = matrix.elements;
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
    //loader.load('./assets/layout.json', function (geometry, materials) {
    //    layout = new THREE.Mesh(geometry, materials);
    //    scene.add(layout);
    //});
    loader.load('./assets/track.json', function (geometry, materials) {
        trackGeometry = geometry;
        trackMaterials = materials;
        //track = new THREE.Mesh(geometry, materials);
        //track.traverse(function (object) {
        //    if (object instanceof THREE.Mesh) {
        //        object.castShadow = true;
        //        object.receiveShadow = true;
        //    }
        //});
        SetupTracksMatrix(geometry);
    });
   // skybox();

    stats = new Stats();
    stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
    document.body.appendChild(stats.dom);
}

function SetupTracksMatrix(geo) {

    var instances = 250;
    // geometry
    bufferGeometry = new THREE.BufferGeometry().fromGeometry(geo);
    geometry = new THREE.InstancedBufferGeometry();

    geometry.index = bufferGeometry.index;
    geometry.attributes.position = bufferGeometry.attributes.position;
    geometry.attributes.uv = bufferGeometry.attributes.uv;

    offsets = [];
    orientations = [];

    for (var i = 0; i < instances; i++) {
        offsets.push(0, 0, 0);
        orientations.push(0, 0, 0, 0);
    }

    offsetAttribute = new THREE.InstancedBufferAttribute(new Float32Array(offsets), 3).setDynamic(true);
    orientationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(orientations), 4).setDynamic(true);
    geometry.addAttribute('offset', offsetAttribute);
    geometry.addAttribute('orientation', orientationAttribute);




    // material
    var vert = document.getElementById('vertexShader').textContent;
    var frag = document.getElementById('fragInstanced').textContent;
    var material = new THREE.RawShaderMaterial({
        vertexShader: vert,
        fragmentShader: frag,
    });

    mesh = new THREE.Mesh(geometry, material);

    scene.add(mesh);
    animate();
}

function CoasterUpdate(added, removed) {

    for (var i = 0; i < removed; i++) {
        //scene.remove(trackMeshs[trackMeshs.length - 1]);
        //trackMeshs.pop();
        trackCount--;
    }
    CreatesTracks(added, false);

}
function CreatesTracks(added, color) {
    for (var i = 0; i < added; i++) {

        console.log("X : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20));
        console.log("Y : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 4));
        console.log("Z : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 8));
        console.log("Yaw : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 12));
        console.log("Pitch : " + Blazor.platform.readFloatField(dataReference, (trackMeshs.length) * 20 + 16));
        CreateTrack(trackCount, trackCount * 20)
        trackCount++;
       // trackMeshs.push(CreateTrack((trackMeshs.length) * 20, false));
    }
    offsetAttribute.needsUpdate = true;
    orientationAttribute.needsUpdate = true; 
}
function CreateTrack(trackCount, trackIndex) {
    //X, Y, Z
    offsetAttribute.array[trackCount * 3] = -Blazor.platform.readFloatField(dataReference, trackIndex) * .036 + 18.5;
    offsetAttribute.array[trackCount * 3 + 1] = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * .036;
    offsetAttribute.array[trackCount * 3 + 2] = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * .036 + 8.55;

   //SIZE

   //YAW

   //PITCH

   //RESOUCES



    //orientationAttribute.array[trackCount * 4 + 3] = Math.random() * 2 - 1;

    //var vector = new THREE.Vector4();
    //vector.set(
    //    0,
    //    Math.random() * 2 - 1,
    //    0,
    //    0
    //).normalize();

    ////Yaw, Pitch
    //orientationAttribute.array[trackCount * 4] = vector.x
    //orientationAttribute.array[trackCount * 4 + 1] = vector.y;
    //orientationAttribute.array[trackCount * 4 + 2] = vector.z;
    //orientationAttribute.array[trackCount * 4 + 3] = vector.w;
}
//function CreateTrackMatrix(trackIndex, matrix) {
//    var position = new THREE.Vector3();
//    var rotation = new THREE.Euler();
//    var quaternion = new THREE.Quaternion();
//    var scale = new THREE.Vector3();

//    position.x = -Blazor.platform.readFloatField(dataReference, trackIndex) * .036 + 18.5;
//    position.y = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * .036;
//    position.z = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * .036 + 8.55;

//    rotation.order = 'ZYX';
//    rotation.x = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 16));;
//    rotation.y = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90);
//    rotation.z = 0;

//    quaternion.setFromEuler(rotation, false);
//    scale.x = scale.y = scale.z = 5;
//    matrix.compose(position, quaternion, scale);
//}
//function randomizeMatrix() {
//    var position = new THREE.Vector3();
//    var rotation = new THREE.Euler();
//    var quaternion = new THREE.Quaternion();
//    var scale = new THREE.Vector3();
//    return function (matrix) {
//        position.x = 0;
//        position.y = 0;
//        position.z = 0;
//        rotation.x = 0;
//        rotation.y = 0;
//        rotation.z = 0;
//        quaternion.setFromEuler(rotation, false);
//        scale.x = scale.y = scale.z = 1;
//        matrix.compose(position, quaternion, scale);
//    };
//}

//    var trackMesh;

//    //Material
//    if (color == true) {
//        material = [{ color: color, wireframe: false }];
//    }
//    else {
//      //  material = [trackMaterials];
//    }

//    //Create Mesh
//    trackMesh = new THREE.Mesh(trackGeometry, trackMaterials);

//    //Scale
//    trackMesh.scale.x = .205;
//    trackMesh.scale.y = .205;
//    trackMesh.scale.z = .205;

//    //X, Y, Z
//    trackMesh.position.x = -Blazor.platform.readFloatField(dataReference, trackIndex) * .036 + 18.5; //X
//    trackMesh.position.y = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * .036; //Z
//    trackMesh.position.z = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * .036 + 8.55; //Y
//    //Data[i * 5] = NewTracks[i].X;
//    //Data[i * 5 + 1] = NewTracks[i].Y;
//    //Data[i * 5 + 2] = NewTracks[i].Z;
//    //Data[i * 5 + 3] = NewTracks[i].Yaw;
//    //Data[i * 5 + 4] = NewTracks[i].Pitch;

//    //Rotate
//    trackMesh.rotation.order = 'ZYX';
//    trackMesh.rotation.x = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 16));  //Pitch
//    trackMesh.rotation.y = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90);
//    trackMesh.rotation.z = 0;

//    //Shadow
//    trackMesh.traverse(function (object) {
//        if (object instanceof THREE.Mesh) {
//            object.castShadow = true;
//            object.receiveShadow = true;
//        }

//    });

//    trackMesh.receiveShadow = true;
//    scene.add(trackMesh);

//    return trackMesh;
//}
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
