using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Burst.CompilerServices;
using Unity.Collections;
//using UnityEditor.VersionControl;
//using Unity.Collections.LowLevel.Unsafe;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnObject;
    private Transform spawnedObject;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData { 
            _int  = 69, 
            _bool = true,
            _message = "Hello World!"
        },
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes _message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref _message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; RandomNumber: " + newValue._int + "; Bool: " + newValue._bool);
        };

    }

    private void Update()
    {
        if (!IsOwner) { return; }

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            spawnedObject = Instantiate(spawnObject);
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);
            //TestServerRpc();
            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
            //randomNumber.Value = new MyCustomData
            //{
            //    _int = 10,
            //    _bool = false,
            //    _message = "Hi potae"
            //};
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            spawnedObject.GetComponent<NetworkObject>().Despawn(true);
            //Destroy(spawnedObject.gameObject);
        }

        Vector3 moveDIr = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDIr.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDIr.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDIr.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDIr.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDIr * moveSpeed * Time.deltaTime;
    }

    // Client send message to server
    [ServerRpc]
    private void TestServerRpc() 
    {
        Debug.Log("OwnerId: " + OwnerClientId + "; TestServerRpc");
    }

    // Server send message to Client
    // Only Send the param to specific client 
    //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams {TargetClientIds = new List<ulong> { 1 } } });
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("OwnerId: " + OwnerClientId + "; TestClientRpc");
    }
}
