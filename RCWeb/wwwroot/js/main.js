let stats;
let controls, camera, scene, renderer, light, loader;
let layoutGeometry, layoutMaterials, layoutMesh;
let trackWoodGeometry, trackWoodBufferGeometry, trackWoodInstancedBufferGeometry, trackWoodMesh;
let trackSupportGeometry, trackSupportBufferGeometry, trackSupportInstancedBufferGeometry, trackSupportMesh;
let scale, translation, rotation;
let translationAttribute, rotationAttribute, scaleAttribute;
let materialSolidColor, materialTexture;
let cameraType = "Auto", activeRender = null, lookAtPostion = null;
let trackCount = 0;
let totalAnimates = 0, times = [], fps;
let trackObjects = [];
const MAX_TRACKS = 25000,
    TRACK_GEOMETRY_SCALE = .19,
    TRACK_POSTION_SCALE = .036,
    TRACK_X_OFFSET = 18.5,
    TRACK_Y_OFFSET = 0;
    TRACK_Z_OFFSET = -12;

init();
function init() {
    initStats();

    initScene();
    initCamera();
    initLights();
    initRenderer();
    initControls();

    loader = new THREE.JSONLoader();
    initLayout();
    initSkybox();
    initTrack();
}

function initScene() {
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0xf0f0f0);
}
function initCamera() {
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.01, 10000);
    camera.position.z = 2;
    camera.position.set(0, 3.5 / 2, 5 / 2);
    camera.lookAt(scene.position);
}
function initLights() {
    light = new THREE.DirectionalLight(0xdfebff, 2.2);
    light.position.set(0, 400, 50);
    light.position.multiplyScalar(1);
    scene.add(light);
}
function initRenderer() {
    renderer = new THREE.WebGLRenderer({ antialias: true });
    rendererTwo = new THREE.WebGLRenderer();
    activeRender = renderer;
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setPixelRatio(window.devicePixelRatio);

    rendererTwo.setSize(window.innerWidth, window.innerHeight);
    rendererTwo.setPixelRatio(window.devicePixelRatio);

    document.body.appendChild(renderer.domElement);
    document.body.appendChild(rendererTwo.domElement);

    THREEx.WindowResize(renderer, camera)
    THREEx.WindowResize(rendererTwo, camera)
}
function initControls() {
    controls = new THREE.OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = .15;
    controls.rotateSpeed = 0.25;
    controls.enableZoom = true;
    controls.maxPolarAngle = Math.PI / 2.1;
    controls.minPolarAngle = 0;

    controls = new THREE.OrbitControls(camera, rendererTwo.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = .15;
    controls.rotateSpeed = 0.25;
    controls.enableZoom = true;
    controls.maxPolarAngle = Math.PI / 2.1;
    controls.minPolarAngle = 0;
}
function initStats() {
    stats = new Stats();
    stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
    document.body.appendChild(stats.dom);
}
function initMesh() {
    loader.load('./assets/layout.json', function (geometry, materials) {
        layout = new THREE.Mesh(geometry, materials);
        scene.add(layout);
    });
}
function initTrack() {
    loader.load('./assets/WoodTrack.json', function (geometry, materials) {
        trackWoodGeometry = geometry;
        if (trackSupportGeometry != null) {
            setupTracksMatrix();
        }
    });
    loader.load('./assets/SupportTrack.json', function (geometry, materials) {
        trackSupportGeometry = geometry;
        if (trackWoodGeometry != null) {
            setupTracksMatrix();
        }
    });
}
function initLayout() {
    loader.load('./assets/Layout.json', function (geometry, materials) {
        layoutGeometry = geometry;
        layoutMaterials = materials;
        layoutGeometry.sortFacesByMaterialIndex();
        layoutMesh = new THREE.Mesh(layoutGeometry, layoutMaterials);
        layoutMesh.scale.x = 6;
        layoutMesh.scale.y = 6;
        layoutMesh.scale.z = 6;

        scene.add(layoutMesh);
    });
}
function initSkybox() {
    var geometry = new THREE.CubeGeometry(10000, 10000, 10000);
    var cubeMaterials = [
        new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("./assets/textures/front.png"), side: THREE.DoubleSide }),
        new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("./assets/textures/back.png"), side: THREE.DoubleSide }),
        new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("./assets/textures/up.png"), side: THREE.DoubleSide }),
        new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("./assets/textures/down.png"), side: THREE.DoubleSide }),
        new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("./assets/textures/right.png"), side: THREE.DoubleSide }),
        new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("./assets/textures/left.png"), side: THREE.DoubleSide }),
    ];
    var cube = new THREE.Mesh(geometry, cubeMaterials);
    scene.add(cube);
}
function setupTracksMatrix() {
    trackWoodBufferGeometry = new THREE.BufferGeometry().fromGeometry(trackWoodGeometry);
    trackWoodInstancedBufferGeometry = new THREE.InstancedBufferGeometry();
    trackWoodInstancedBufferGeometry.maxInstancedCount = 0;
    trackWoodInstancedBufferGeometry.index = trackWoodBufferGeometry.index;
    trackWoodInstancedBufferGeometry.attributes.position = trackWoodBufferGeometry.attributes.position;
    trackWoodInstancedBufferGeometry.attributes.uv = trackWoodBufferGeometry.attributes.uv;

    trackSupportBufferGeometry = new THREE.BufferGeometry().fromGeometry(trackSupportGeometry);
    trackSupportInstancedBufferGeometry = new THREE.InstancedBufferGeometry();
    trackSupportInstancedBufferGeometry.maxInstancedCount = 0;
    trackSupportInstancedBufferGeometry.index = trackSupportBufferGeometry.index;
    trackSupportInstancedBufferGeometry.attributes.position = trackSupportBufferGeometry.attributes.position;
    trackSupportInstancedBufferGeometry.attributes.uv = trackSupportBufferGeometry.attributes.uv;

    scale = [];
    translation = [];
    rotation = [];

    for (var i = 0; i < MAX_TRACKS; i++) {
        scale.push(TRACK_GEOMETRY_SCALE, TRACK_GEOMETRY_SCALE, TRACK_GEOMETRY_SCALE);
        translation.push(0, 0, 0);
        rotation.push(0, 0, 0, 1);
        trackObjects.push({
            x: 0,
            y: 0,
            z: 0,
            yaw: 0,
            pitch: 0,
            quaternion: {
                x: 0,
                y: 0,
                z: 0,
                w: 1
            }
        })
    }

    translationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(translation), 3);
    rotationAttribute = new THREE.InstancedBufferAttribute(new Float32Array(rotation), 4);
    scaleAttribute = new THREE.InstancedBufferAttribute(new Float32Array(scale), 3);

    trackWoodInstancedBufferGeometry.addAttribute('translation', translationAttribute);
    trackWoodInstancedBufferGeometry.addAttribute('rotation', rotationAttribute);
    trackWoodInstancedBufferGeometry.addAttribute('scale', scaleAttribute);

    trackSupportInstancedBufferGeometry.addAttribute('translation', translationAttribute);
    trackSupportInstancedBufferGeometry.addAttribute('rotation', rotationAttribute);
    trackSupportInstancedBufferGeometry.addAttribute('scale', scaleAttribute);

    // create a material
    var vertexShader = document.getElementById('vertexShader').textContent;
    var fragmentShader = document.getElementById('fragmentShader').textContent;
    var vertexTextureShader = document.getElementById('vertexTextureShader').textContent;
    var fragmentTextureShader = document.getElementById('fragmentTextureShader').textContent;

    materialSolidColor = new THREE.RawShaderMaterial({
        vertexShader: vertexShader,
        fragmentShader: fragmentShader,
    });
    materialTexture = new THREE.RawShaderMaterial({
        uniforms: {
            map: { value: new THREE.TextureLoader().load('./assets/textures/wood.png') }
        },
        vertexShader: vertexTextureShader,
        fragmentShader: fragmentTextureShader,
    });

    trackWoodMesh = new THREE.Mesh(trackWoodInstancedBufferGeometry, materialTexture);
    trackSupportMesh = new THREE.Mesh(trackSupportInstancedBufferGeometry, materialSolidColor);
    scene.add(trackWoodMesh);
    scene.add(trackSupportMesh);
    animate();

    //If FPS Drops Below 25 For on avg over 5 secounds, Use faster Renders
    window.setInterval(function () {
        if ((totalAnimates / 5) < 25 && activeRender === renderer) {
            activeRender = rendererTwo;
            renderer.domElement.hidden = true;
            clearInterval();
        }
        totalAnimates = 0;
    }, 5000);
}

function updateCoaster(added, removed) {
    for (var i = 0; i < removed; i++) {
        trackCount--;
    }
    createTracks(added, false);
    updateCamera();
}
function updateCamera(){
    if (cameraType == "Auto") {
        //camera.lookAt(
        //    translationVector[trackCount - 1].x,
        //    translationVector[trackCount - 1].y,
        //    translationVector[trackCount - 1].z
        //        );
    }
}

function createTracks(added, color) {
    for (var i = 0; i < added; i++) {
        createTrack(trackCount, trackCount * 20)

        console.log("");
        console.log("RC");
        console.log("X : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20));
        console.log("Y : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 4));
        console.log("Z : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 8));
        console.log("Yaw : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 12));
        console.log("Pitch : " + Blazor.platform.readFloatField(dataReference, (trackCount) * 20 + 16));

        console.log("");
        console.log("WEBGL");
        console.log("X : " + trackObjects[trackCount].x);
        console.log("Y : " + trackObjects[trackCount].y);
        console.log("Z : " + trackObjects[trackCount].z);
        console.log("Yaw : " + trackObjects[trackCount].yaw);
        console.log("Pitch : " + trackObjects[trackCount].pitch);

        trackCount++;
    }
    translationAttribute.needsUpdate = true;
    rotationAttribute.needsUpdate = true; 
}
function createTrack(trackCount, trackIndex) {
    //Create Track Object (Stores Relevent Track Data Translated For Three.Js)
    trackObjects[trackCount].x = -Blazor.platform.readFloatField(dataReference, trackIndex) * TRACK_POSTION_SCALE + TRACK_X_OFFSET;
    trackObjects[trackCount].y = Blazor.platform.readFloatField(dataReference, trackIndex + 8) * TRACK_POSTION_SCALE + TRACK_Y_OFFSET;
    trackObjects[trackCount].z = Blazor.platform.readFloatField(dataReference, trackIndex + 4) * TRACK_POSTION_SCALE + TRACK_Z_OFFSET;
    trackObjects[trackCount].yaw = Blazor.platform.readFloatField(dataReference, trackIndex + 12) + 90;
    trackObjects[trackCount].pitch = Blazor.platform.readFloatField(dataReference, trackIndex + 16);

    var e = new THREE.Euler();
    e.order = 'ZYX';
    e.x = THREE.Math.degToRad(trackObjects[trackCount].pitch);
    e.y = THREE.Math.degToRad(trackObjects[trackCount].yaw);
    e.z = 0;
    var quaternion = new THREE.Quaternion();
    quaternion.setFromEuler(e, false);

    trackObjects[trackCount].quaternion.x = quaternion.x;
    trackObjects[trackCount].quaternion.y = quaternion.y;
    trackObjects[trackCount].quaternion.z = quaternion.z;
    trackObjects[trackCount].quaternion.w = quaternion.w;

    //Set Attributes
    translationAttribute.array[trackCount * 3] = trackObjects[trackCount].x;
    translationAttribute.array[trackCount * 3 + 1] = trackObjects[trackCount].y;
    translationAttribute.array[trackCount * 3 + 2] = trackObjects[trackCount].z;

    rotationAttribute.array[trackCount * 4] = trackObjects[trackCount].quaternion.x
    rotationAttribute.array[trackCount * 4 + 1] = trackObjects[trackCount].quaternion.y;
    rotationAttribute.array[trackCount * 4 + 2] = trackObjects[trackCount].quaternion.z;
    rotationAttribute.array[trackCount * 4 + 3] = trackObjects[trackCount].quaternion.w;
}

function animate(time) {
    totalAnimates++;
    const now = performance.now();
    while (times.length > 0 && times[0] <= now - 1000) {
        times.shift();
    }
    times.push(now);
    fps = times.length;
    
    stats.begin();
    controls.update();

    trackCulling();

    TWEEN.update(time);

    activeRender.render(scene, camera);
    stats.end();

    requestAnimationFrame(animate);
}
function trackCulling() {
    const frustum = new THREE.Frustum();
    const projScreenMatrix = new THREE.Matrix4();

    projScreenMatrix.multiplyMatrices(
        camera.projectionMatrix,
        camera.matrixWorldInverse
    );
    frustum.setFromMatrix(projScreenMatrix);

    let visibleInstanceCount = 0;

    for (let i = 0; i < trackCount; i++) {
        let trackObject = trackObjects[i];
        if (!frustum.intersectsBox(new THREE.Box3(new THREE.Vector3(trackObject.x - .2, trackObject.y - .2, trackObject.z - .2), new THREE.Vector3(trackObject.x + .2, trackObject.y + .2, trackObject.z + .2)))) {
            continue;
        }
        
        translationAttribute.array[visibleInstanceCount * 3] = trackObject.x;
        translationAttribute.array[visibleInstanceCount * 3 + 1] = trackObject.y;
        translationAttribute.array[visibleInstanceCount * 3 + 2] = trackObject.z;

        rotationAttribute.array[visibleInstanceCount * 4] = trackObject.quaternion.x
        rotationAttribute.array[visibleInstanceCount * 4 + 1] = trackObject.quaternion.y;
        rotationAttribute.array[visibleInstanceCount * 4 + 2] = trackObject.quaternion.z;
        rotationAttribute.array[visibleInstanceCount * 4 + 3] = trackObject.quaternion.w;

        visibleInstanceCount++;
    }
    translationAttribute.needsUpdate = true;
    rotationAttribute.needsUpdate = true; 
    trackWoodInstancedBufferGeometry.maxInstancedCount = visibleInstanceCount;
    trackSupportInstancedBufferGeometry.maxInstancedCount = visibleInstanceCount;

    document.getElementById("TracksRendering").textContent = 'Tracks Rendering: ' + visibleInstanceCount;
}