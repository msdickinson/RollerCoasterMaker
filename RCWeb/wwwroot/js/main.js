var controls, camera, scene, renderer, loader;
var layout, track, trackMeshs, trackGeometry, trackMaterials;
var stats;
var trackCount;
var objectCount, instanceCount = 100, tracksMatrix, mcol0, mcol1, mcol2, mcol3, tracksMatrixMax = 1000, matrix, me, bgeo;
var mcol0, mcol1, mcol2, mcol3;
var bufferGeometry, geometry, offsets = [], orientations = [], euler;
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
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.01, 5000);
    camera.position.z = 2;
    camera.position.set(0, 3.5/2, 5/2);
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

function SetupTracksMatrix(geo, materials) {
    var count = 1000;
   // geo.sortFacesByMaterialIndex();
    bufferGeometry = new THREE.BufferGeometry().fromGeometry(geo);


    geometry = new THREE.InstancedBufferGeometry();

    geometry.index = bufferGeometry.index;
    geometry.attributes.position = bufferGeometry.attributes.position;
    geometry.attributes.uv = bufferGeometry.attributes.uv;

    scale = [];
    translation = [];
    rotation = [];
    euler = []
    var euler = new THREE.Euler();
    for (var i = 0; i < count; i++) {
        translation.push(10000, 10000, 10000);
        scale.push(.205, .205, .205);
        var e = new THREE.Euler();

        e.order = 'ZYX';
        e.x = THREE.Math.degToRad(0); // Pitch
        e.y = THREE.Math.degToRad(0 + 90); // Yaw
        e.z = 0;

        var quaternion = new THREE.Quaternion();
        quaternion.setFromEuler(e, false);
        
        rotation.push(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }

    //create the InstancedBufferAttributes from our float buffers

    //setDynamic(true) ?
    translationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(translation), 3);
    rotationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(rotation), 4);
    scaleAttribute = new THREE.InstancedBufferAttribute(new Float32Array(scale), 3);
    geometry.addAttribute('translation', translationAttribute);
    geometry.addAttribute('rotation', rotationAttribute);
    geometry.addAttribute('scale', scaleAttribute);

    // create a material
    var vertexShader = document.getElementById('vertexShader').textContent;
    var fragmentShader = document.getElementById('fragmentShader').textContent;
    var vertexTextureShader = document.getElementById('vertexTextureShader').textContent;
    var fragmentTextureShader = document.getElementById('fragmentTextureShader').textContent;

    var material = new THREE.RawShaderMaterial({
        vertexShader: vertexShader,
        fragmentShader: fragmentShader,
    });
    var materialTexture = new THREE.RawShaderMaterial({
        uniforms: {
            map: { value: new THREE.TextureLoader().load('./assets/textures/wood.png') }
        },
        vertexShader: vertexTextureShader,
        fragmentShader: fragmentTextureShader,
    });
    mesh = new THREE.Mesh(geometry, materialTexture );

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
    translationAttribute.needsUpdate = true;
    rotationAttribute.needsUpdate = true; 
}
function CreateTrack(trackCount, trackIndex) {
    //translationAttribute
    translationAttribute.array[trackCount * 3] = -Blazor.platform.readFloatField(dataReference, trackIndex) * .036 + 18.5;
    translationAttribute.array[trackCount * 3 + 1] = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * .036;
    translationAttribute.array[trackCount * 3 + 2] = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * .036 + 8.55;

   ////rotationAttribute
    var e = new THREE.Euler();

    e.order = 'ZYX';
    e.x = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 16));;
    e.y = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90);
    e.z = 0;

    var quaternion = new THREE.Quaternion();
    quaternion.setFromEuler(e, false);

    rotationAttribute.array[trackCount * 4] = quaternion.x
    rotationAttribute.array[trackCount * 4 + 1] = quaternion.y;
    rotationAttribute.array[trackCount * 4 + 2] = quaternion.z;
    rotationAttribute.array[trackCount * 4 + 3] = quaternion.w;
  //  rotation
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
