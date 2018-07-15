var controls, camera, scene, renderer, loader;
var layout, track, trackMeshs, trackGeometry, trackMaterials;
var stats;
var trackCount;
var objectCount, instanceCount = 100, tracksMatrix, mcol0, mcol1, mcol2, mcol3, tracksMatrixMax = 1000, matrix, me, bgeo;
var mcol0, mcol1, mcol2, mcol3;
var bufferWoodGeometry, geometryWood, offsets = [], orientations = [], euler;
var bufferSupportGeometry, geometrySupport, offsets = [], orientations = [], euler;
var translationVector, rotationVector, rendererTwo;
var kOffSets, offsetAttribute, orientationAttribute;

var trackWoodGeometry = null,
    trackWoodMaterials = null,
    trackSupportGeometry = null,
    trackSupportMaterials = null;
var activeRender = null;
init();

function init() {

    var isMobile = false; //initiate as false
    // device detection
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        isMobile = true;
    }


    trackCount = 0;
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0xf0f0f0); // UPDATED
    matrix = new THREE.Matrix4();
    me = matrix.elements;
    /* Camera */
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.01, 10000);
    camera.position.z = 2;
    camera.position.set(0, 3.5/2, 5/2);
    camera.lookAt(scene.position);

    /* Lights */
    initLights();
    renderer = new THREE.WebGLRenderer({ antialias: true });
    rendererTwo = new THREE.WebGLRenderer();
    activeRender = renderer;
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setPixelRatio(window.devicePixelRatio);

    rendererTwo.setSize(window.innerWidth, window.innerHeight);
    rendererTwo.setPixelRatio(window.devicePixelRatio);
    
    document.body.appendChild(renderer.domElement);
    document.body.appendChild(rendererTwo.domElement);

	/* Resize */
	THREEx.WindowResize(renderer, camera)
    THREEx.WindowResize(rendererTwo, camera)
	/* Controls */
	controls = new THREE.OrbitControls(camera, renderer.domElement);
	controls.enableDamping = true;
	controls.dampingFactor = .15;
	controls.rotateSpeed = 0.25; 
	controls.enableZoom = true;
	controls.maxPolarAngle  = Math.PI / 2.1;
    controls.minPolarAngle = 0;

    /* Controls */
    controls = new THREE.OrbitControls(camera, rendererTwo.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = .15;
    controls.rotateSpeed = 0.25;
    controls.enableZoom = true;
    controls.maxPolarAngle = Math.PI / 2.1;
    controls.minPolarAngle = 0;

	/* Events */
	//window.addEventListener('resize', onWindowResize, false);

	

	/* Load Models */
    trackMeshs = new Array();
    var loader = new THREE.JSONLoader();
    loader.load('./assets/LayoutGoodSize3Join.json', function (geometry, materials) {
        geometry.sortFacesByMaterialIndex();
        layout = new THREE.Mesh(geometry, materials);

        layout.scale.x = 6;
        layout.scale.y = 6;
        layout.scale.z = 6;

        layout.traverse(function (object) {
            if (object instanceof THREE.Mesh) {
                object.castShadow = true;
                object.receiveShadow = true;
            }
        });
        scene.add(layout);
    });
    loader.load('./assets/WoodTrack.json', function (geometry, materials) {
        trackWoodGeometry = geometry;
        trackWoodMaterials = materials;

        SetupTracksMatrix();
    });
    loader.load('./assets/SupportTrack.json', function (geometry, materials) {
        trackSupportGeometry = geometry;
        trackSupportMaterials = materials;
        SetupTracksMatrix();
    });
    skybox();

    stats = new Stats();
    stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
    document.body.appendChild(stats.dom);
}

function SetupTracksMatrix(geo1, materials1, geo2, materials2) {
    var count = 10000;
   // geo.sortFacesByMaterialIndex();
    if (trackWoodGeometry == null ||
        trackWoodMaterials == null ||
        trackSupportGeometry == null ||
        trackSupportMaterials == null) {
        return;
    }
    //trackWoodGeometry.sortFacesByMaterialIndex();
    //trackSupportGeometry.sortFacesByMaterialIndex();
    bufferWoodGeometry = new THREE.BufferGeometry().fromGeometry(trackWoodGeometry);
    bufferSupportGeometry = new THREE.BufferGeometry().fromGeometry(trackSupportGeometry);

    geometryWood = new THREE.InstancedBufferGeometry();
    geometryWood.index = bufferWoodGeometry.index;
    geometryWood.attributes.position = bufferWoodGeometry.attributes.position;
    geometryWood.attributes.uv = bufferWoodGeometry.attributes.uv;

    geometrySupport = new THREE.InstancedBufferGeometry();
    geometrySupport.index = bufferSupportGeometry.index;
    geometrySupport.attributes.position = bufferSupportGeometry.attributes.position;
    geometrySupport.attributes.uv = bufferSupportGeometry.attributes.uv;

    scale = [];
    translation = [];
    translationVector = []
    rotation = [];
    rotationVector = [];
    euler = []
    var euler = new THREE.Euler();
    for (var i = 0; i < count; i++) {
        translation.push(0, 0, 0);
        translationVector.push(new THREE.Vector3(0, 0, 0));
        scale.push(.19, .19, .19);
        var e = new THREE.Euler();

        e.order = 'ZYX';
        e.x = THREE.Math.degToRad(0); // Pitch
        e.y = THREE.Math.degToRad(0 + 90); // Yaw
        e.z = 0;

        var quaternion = new THREE.Quaternion();
        quaternion.setFromEuler(e, false);
        
        rotation.push(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        rotationVector.push(new THREE.Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w));
    }

    //create the InstancedBufferAttributes from our float buffers

    //setDynamic(true) ?
    translationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(translation), 3);
    rotationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(rotation), 4);
    scaleAttribute = new THREE.InstancedBufferAttribute(new Float32Array(scale), 3);


    geometryWood.addAttribute('translation', translationAttribute);
    geometryWood.addAttribute('rotation', rotationAttribute);
    geometryWood.addAttribute('scale', scaleAttribute);
    geometrySupport.addAttribute('translation', translationAttribute);
    geometrySupport.addAttribute('rotation', rotationAttribute);
    geometrySupport.addAttribute('scale', scaleAttribute);

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

    meshWood = new THREE.Mesh(geometryWood, materialTexture);
    meshSupport = new THREE.Mesh(geometrySupport, material);
    geometryWood.maxInstancedCount = 1;
    geometrySupport.maxInstancedCount = 1;
    scene.add(meshWood);
    scene.add(meshSupport);
    animate();




    window.setInterval(function () {
        if ((totalAnimates / 5) < 25 && activeRender === renderer) {
            activeRender = rendererTwo;
            renderer.domElement.hidden = true;
            clearInterval();
        }
        totalAnimates = 0;
    }, 5000);
}

function CoasterUpdate(added, removed) {

    for (var i = 0; i < removed; i++) {
        //scene.remove(trackMeshs[trackMeshs.length - 1]);
        //trackMeshs.pop();
        trackCount--;
    }
    CreatesTracks(added, false);

}


//   *    ]   *    ]    *
function CreatesTracks(added, color) {

    for (var i = 0; i < added; i++) {

        console.log("X : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20));
        console.log("Y : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 4));
        console.log("Z : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 8));
        console.log("Yaw : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 12));
        console.log("Pitch : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 16));
        CreateTrack(trackCount, trackCount * 20)
        trackCount++;
       // trackMeshs.push(CreateTrack((trackMeshs.length) * 20, false));
    }
    translationAttribute.needsUpdate = true;
    rotationAttribute.needsUpdate = true; 
   // geometryWood.maxInstancedCount = trackCount;
   // geometrySupport.maxInstancedCount = trackCount;
}
function CreateTrack(trackCount, trackIndex) {
    //translationAttribute
    translationAttribute.array[trackCount * 3] = -Blazor.platform.readFloatField(dataReference, trackIndex) * .036 + 18.5;
    translationAttribute.array[trackCount * 3 + 1] = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * .036;
    translationAttribute.array[trackCount * 3 + 2] = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * .036 - 12;
    translationVector[trackCount] = new THREE.Vector3(
        translationAttribute.array[trackCount * 3],
        translationAttribute.array[trackCount * 3 + 1],
        translationAttribute.array[trackCount * 3 + 2]);
    ////rotationAttribute
    var e = new THREE.Euler();

    e.order = 'ZYX';
    e.x = THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 16));
    e.y = THREE.Math.degToRad((Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90) * Math.PI / 180);
    e.z = 0;

    var quaternion = new THREE.Quaternion();
    quaternion.setFromAxisAngle(new THREE.Vector3(0, 1, 0), THREE.Math.degToRad(Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90));
    //quaternion.setFromEuler(e, false);

    rotationAttribute.array[trackCount * 4] = quaternion.x
    rotationAttribute.array[trackCount * 4 + 1] = quaternion.y;
    rotationAttribute.array[trackCount * 4 + 2] = quaternion.z;
    rotationAttribute.array[trackCount * 4 + 3] = quaternion.w;


    rotationVector[trackCount].x = quaternion.x
    rotationVector[trackCount].y = quaternion.y;
    rotationVector[trackCount].z = quaternion.z;
    rotationVector[trackCount].w = quaternion.w;
}

let totalAnimates = 0;
const times = [];
let fps;
function animate() {
    totalAnimates++;
    const now = performance.now();
    while (times.length > 0 && times[0] <= now - 1000) {
        times.shift();
    }
    times.push(now);
    fps = times.length;
    //if (fps < 30 && activeRender === renderer) {
    //    activeRender = rendererTwo;
    //    renderer.domElement.hidden = true
    //}

    stats.begin();
    controls.update();

    determineTracksToRender();

 
    activeRender.render(scene, camera);
    stats.end();

    requestAnimationFrame(animate);
}
function determineTracksToRender() {

    const frustum = new THREE.Frustum();
    const projScreenMatrix = new THREE.Matrix4();

    // for every frame
    projScreenMatrix.multiplyMatrices(
        camera.projectionMatrix,
        camera.matrixWorldInverse
    );
    frustum.setFromMatrix(projScreenMatrix);

    let visibleInstanceCount = 0;

    for (let i = 0; i < trackCount; i++) {
        const pos = translationVector[i];
        const rot = rotationVector[i];
        //Scale is .205 3.7 .2
        if (!frustum.intersectsBox(new THREE.Box3(new THREE.Vector3(pos.x - .2, pos.y - .2, pos.z - .2), new THREE.Vector3(pos.x + .2, pos.y + .2, pos.z + .2)))) {
            continue;
        }
        
        // add instance to instance-attribute buffers
        pos.toArray(translationAttribute.array, visibleInstanceCount * 3);

        rotationAttribute.array[visibleInstanceCount * 4] = rot.x
        rotationAttribute.array[visibleInstanceCount * 4 + 1] = rot.y;
        rotationAttribute.array[visibleInstanceCount * 4 + 2] = rot.z;
        rotationAttribute.array[visibleInstanceCount * 4 + 3] = rot.w;
        visibleInstanceCount++;
    }
    translationAttribute.needsUpdate = true;
    rotationAttribute.needsUpdate = true; 
    document.getElementById("TracksRendering").textContent = 'Tracks Rendering: ' + visibleInstanceCount;
    geometryWood.maxInstancedCount = visibleInstanceCount;
    geometrySupport.maxInstancedCount = visibleInstanceCount;
}
function initLights() {
    //var light = new THREE.AmbientLight(0xffffff, 0.3);
    //scene.add(light);

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
