package mygame;

import com.jme3.math.Vector3f;
import com.jme3.math.Ray;
import com.jme3.scene.Node;
import com.jme3.scene.Spatial;
import com.simsilica.es.EntityComponent;
import com.simsilica.es.EntityId;

public class SteeringBehaviours implements EntityComponent{
    EntityId id;
    
    public SteeringBehaviours(){
        
    }
    
    public SteeringBehaviours getValue(){
        return this;
    }
    
    public static final float Random(float min, float max){
        max = (float) (max * Math.random());
        return min + max;
    }
    
    public Vector3f Seek(Spatial self, Spatial target){
        return (target.getWorldTranslation().subtract(self.getWorldTranslation()).normalize());
    }
    
    public Vector3f Seek(Vector3f self, Vector3f target){
        return (target.subtract(self).normalize());
    }
    
    public Vector3f Wander(Vector3f self, float r){
        return new Vector3f(self.x + Random(0.0f,r), self.y + Random(0.0f,r), self.z + Random(0.0f,r));
    }
    
    public Vector3f Flee(Spatial self, Spatial target){
        return self.getWorldTranslation().subtract(target.getWorldTranslation()).normalize();
    }
    
    public Vector3f Flee(Vector3f self, Vector3f target){
        return self.subtract(target).normalize();
    }
    
    public Vector3f Avoid(Spatial self, Spatial[] others, float maxDistance){
        Vector3f dir = Vector3f.ZERO;
        Vector3f ahead = self.getWorldTranslation().add(Vector3f.UNIT_Z);
        
        for(Spatial s : others)
            if(s.getWorldTranslation().distance(ahead) > maxDistance/2)
                ahead.subtract(s.getWorldTranslation());
        return dir.normalize();
    }
    
    public Vector3f Arrive(){
        
    }
}
